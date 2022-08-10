Public Module World
    Public Sub Start()
        StaticStore.Store.Reset()
        CreateTown()
        CreateDungeon(Location.FromLocationType(OldLocationType.ChurchEntrance).First)
        CreateMoon()
        CreateFeatures()
        CreatePlayer()
    End Sub

    Private Sub CreateMoon()
        Dim locations As New List(Of Location)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim dungeonLocation = Location.Create(OldLocationType.Moon)
                dungeonLocation.DungeonLevel = DungeonLevel.FromName(TheMoon)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonColumn, column)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonRow, row)
                locations.Add(dungeonLocation)
            Next
        Next
        Dim north = Direction.FromName(DirectionUtility.North)
        Dim east = Direction.FromName(DirectionUtility.East)
        Dim south = Direction.FromName(DirectionUtility.South)
        Dim west = Direction.FromName(DirectionUtility.West)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim moonLocation = locations(CInt(column + row * MoonColumns))
                Dim northLocation = locations(CInt(column + ((row + MoonRows - 1) Mod MoonRows) * MoonColumns))
                Dim southLocation = locations(CInt(column + ((row + 1) Mod MoonRows) * MoonColumns))
                Dim eastLocation = locations(CInt(((column + 1) Mod MoonColumns) + row * MoonColumns))
                Dim westLocation = locations(CInt(((column + MoonColumns - 1) Mod MoonColumns) + row * MoonColumns))
                Route.Create(moonLocation, north, RouteType.MoonPath, northLocation)
                Route.Create(moonLocation, south, RouteType.MoonPath, southLocation)
                Route.Create(moonLocation, east, RouteType.MoonPath, eastLocation)
                Route.Create(moonLocation, west, RouteType.MoonPath, westLocation)
            Next
        Next
        PopulateCharacters(locations, DungeonLevel.FromName(TheMoon))
        PopulateItems(locations, DungeonLevel.FromName(TheMoon))
    End Sub

    Private Sub CreateDungeon(location As Location)
        location = CreateDungeonLevel(location, DungeonLevel.FromName(Level1), ItemType.CopperKey, RouteType.CopperLock) 'TODO: add "reward item type" and "boss character type"
        location = CreateDungeonLevel(location, DungeonLevel.FromName(Level2), ItemType.SilverKey, RouteType.SilverLock)
        location = CreateDungeonLevel(location, DungeonLevel.FromName(Level3), ItemType.GoldKey, RouteType.GoldLock)
        location = CreateDungeonLevel(location, DungeonLevel.FromName(Level4), ItemType.PlatinumKey, RouteType.PlatinumLock)
        location = CreateDungeonLevel(location, DungeonLevel.FromName(Level5), ItemType.ElementalOrb, RouteType.FinalLock)
    End Sub

    Const MazeColumns = 11
    Const MazeRows = 11
    Const MoonColumns = 11
    Const MoonRows = 11
    ReadOnly MazeDirections As IReadOnlyDictionary(Of String, MazeDirection(Of String)) =
        New Dictionary(Of String, MazeDirection(Of String)) From
        {
            {"North", New MazeDirection(Of String)("South", 0, -1)},
            {"East", New MazeDirection(Of String)("West", 1, 0)},
            {"South", New MazeDirection(Of String)("North", 0, 1)},
            {"West", New MazeDirection(Of String)("East", -1, 0)}
        }

    Public ReadOnly Property IsValid As Boolean
        Get
            Return StaticWorldData.World.Player.Read.HasValue
        End Get
    End Property

    Private Function CreateLocations(maze As Maze(Of String), dungeonLevel As DungeonLevel) As IReadOnlyList(Of Location)
        Dim locations As New List(Of Location)
        For row As Long = 0 To maze.Rows - 1
            For column As Long = 0 To maze.Columns - 1
                Dim dungeonLocation = Location.Create(OldLocationType.Dungeon)
                dungeonLocation.DungeonLevel = dungeonLevel
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonColumn, column)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonRow, row)
                locations.Add(dungeonLocation)
            Next
        Next
        For row As Long = 0 To maze.Rows - 1
            For column As Long = 0 To maze.Columns - 1
                Dim cell = maze.GetCell(column, row)
                Dim dungeonLocation = locations(CInt(column + row * maze.Columns))
                For Each direction In MazeDirections.Keys
                    If cell.OpenDoorCount = 1 Then
                        dungeonLocation.LocationType = OldLocationType.DungeonDeadEnd
                    End If
                    If If(cell.GetDoor(direction)?.Open, False) Then
                        Dim nextColumn = MazeDirections(direction).DeltaX + column
                        Dim nextRow = MazeDirections(direction).DeltaY + row
                        Dim nextLocation = locations(CInt(nextColumn + nextRow * maze.Columns))
                        Route.Create(dungeonLocation, Game.Direction.FromName(direction), RouteType.Passageway, nextLocation)
                    End If
                Next
            Next
        Next
        Return locations
    End Function

    Private Function CreateDungeonLevel(fromLocation As Location, dungeonLevel As DungeonLevel, bossKeyType As ItemType, bossRouteType As RouteType) As Location
        Dim maze = New Maze(Of String)(MazeColumns, MazeRows, MazeDirections)
        maze.Generate()
        Dim locations = CreateLocations(maze, dungeonLevel)
        PopulateLocations(locations, bossKeyType, bossRouteType, dungeonLevel)
        Dim startingLocation = RNG.FromEnumerable(locations.Where(Function(x) x.RouteCount > 1))
        Route.Create(fromLocation, Direction.FromName("Down"), RouteType.Stairs, startingLocation)
        Route.Create(startingLocation, Direction.FromName("Up"), RouteType.Stairs, fromLocation)
        PopulateCharacters(locations, dungeonLevel)
        Return locations.Single(Function(x) x.LocationType = OldLocationType.DungeonBoss)
    End Function

    Private Sub PopulateCharacters(locations As IEnumerable(Of Location), dungeonLevel As DungeonLevel)
        For Each characterType In AllCharacterTypes()
            Dim characterCount = characterType.SpawnCount(dungeonLevel)
            While characterCount > 0
                Dim location = RNG.FromEnumerable(locations.Where(Function(x) characterType.CanSpawn(x, dungeonLevel)))
                Character.Create(characterType, location)
                characterCount -= 1
            End While
        Next
    End Sub

    Private Sub PopulateLocations(locations As IReadOnlyList(Of Location), bossKeyType As ItemType, bossRouteType As RouteType, dungeonLevel As DungeonLevel)
        Dim partitions =
            locations.GroupBy(
                Function(x) x.RouteCount = 1).
                    ToDictionary(Of OldLocationType, List(Of Location))(
                        Function(x) If(x.Key, OldLocationType.DungeonDeadEnd, OldLocationType.Dungeon),
                        Function(x) x.ToList)
        Dim deadEnds = partitions(OldLocationType.DungeonDeadEnd)
        Dim nonDeadEnds = partitions(OldLocationType.Dungeon)
        Dim itemTypes As New List(Of ItemType)
        For Each deadEnd In deadEnds
            Dim direction = deadEnd.RouteDirections.First
            Dim nextLocation = deadEnd.Routes(direction).ToLocation
            Dim route = nextLocation.Routes(direction.Opposite)
            route.RouteType = RouteType.IronLock
            itemTypes.Add(ItemType.IronKey)
        Next
        itemTypes(0) = bossKeyType
        Dim bossLocation = PlaceBossLocation(deadEnds, bossRouteType)
        partitions(OldLocationType.DungeonDeadEnd).Remove(bossLocation)
        partitions.Add(OldLocationType.DungeonBoss, New List(Of Location) From {bossLocation})
        For Each itemType In itemTypes
            SpawnItem(locations, dungeonLevel, itemType)
        Next
        PopulateItems(locations, dungeonLevel)
    End Sub

    Private Sub SpawnItem(locations As IReadOnlyList(Of Location), dungeonLevel As DungeonLevel, itemType As ItemType)
        Dim locationTypes = itemType.SpawnLocationTypes(dungeonLevel.Id)
        If locationTypes.Any Then
            Dim spawnLocation = RNG.FromEnumerable(locations.Where(Function(x) locationTypes.Contains(x.LocationType)))
            spawnLocation.Inventory.Add(Item.Create(itemType))
        End If
    End Sub

    Private Sub PopulateItems(locations As IReadOnlyList(Of Location), dungeonLevel As DungeonLevel)
        For Each itemType In AllItemTypes
            Dim itemCount As Long = itemType.RollSpawnCount(dungeonLevel.Id)
            While itemCount > 0
                itemCount -= 1
                SpawnItem(locations, dungeonLevel, itemType)
            End While
        Next
    End Sub

    Private Function PlaceBossLocation(deadEnds As IEnumerable(Of Location), routeType As RouteType) As Location
        Dim bossLocation = RNG.FromEnumerable(deadEnds)
        bossLocation.LocationType = OldLocationType.DungeonBoss
        Dim direction = bossLocation.RouteDirections.First
        Dim nextLocation = bossLocation.Routes(direction).ToLocation
        nextLocation.Routes(direction.Opposite).RouteType = routeType
        Return bossLocation
    End Function

    Private Sub CreateFeatures()
        For Each featureType In AllFeatureTypes
            CreateFeature(featureType)
        Next
    End Sub

    Private Sub CreateFeature(featureType As FeatureType)
        Dim featureLocation = RNG.FromEnumerable(Location.FromLocationType(featureType.LocationType).Where(Function(x) Not x.HasFeature))
        Feature.Create(featureType, featureLocation)
        If featureType = FeatureType.InnKeeper Then
            CreateCellar(featureLocation)
        End If
    End Sub

    Private Sub CreateCellar(fromLocation As Location)
        Dim cellar = Location.Create(OldLocationType.Cellar)
        Route.Create(fromLocation, Direction.FromName("Down"), RouteType.Stairs, cellar)
        Route.Create(cellar, Direction.FromName("Up"), RouteType.Stairs, fromLocation)
    End Sub

    Private Sub CreatePlayer()
        Dim startingLocation = Location.FromLocationType(OldLocationType.TownSquare).First
        Dim playerCharacter = Character.Create(CharacterType.N00b, startingLocation)
        playerCharacter.Location = startingLocation 'to track that this place has been visited
        StaticWorldData.World.Player.Write(playerCharacter.Id, RNG.FromEnumerable(CardinalDirections).Id, PlayerMode.Neutral)
        RollUpPlayerCharacter()
    End Sub

    Private Sub CreateTown()
        Dim centerTown = Location.Create(OldLocationType.TownSquare)
        Dim northTown = Location.Create(OldLocationType.Town)
        Dim northEastTown = Location.Create(OldLocationType.Town)
        Dim eastTown = Location.Create(OldLocationType.Town)
        Dim southEastTown = Location.Create(OldLocationType.Town)
        Dim southTown = Location.Create(OldLocationType.Town)
        Dim southWestTown = Location.Create(OldLocationType.Town)
        Dim westTown = Location.Create(OldLocationType.Town)
        Dim northWestTown = Location.Create(OldLocationType.Town)

        StitchTown(centerTown, Direction.FromName(North), northTown)
        StitchTown(centerTown, Direction.FromName(East), eastTown)
        StitchTown(centerTown, Direction.FromName(South), southTown)
        StitchTown(centerTown, Direction.FromName(West), westTown)

        StitchTown(northWestTown, Direction.FromName(East), northTown)
        StitchTown(northWestTown, Direction.FromName(South), westTown)

        StitchTown(southWestTown, Direction.FromName(East), southTown)
        StitchTown(southWestTown, Direction.FromName(North), westTown)

        StitchTown(northEastTown, Direction.FromName(West), northTown)
        StitchTown(northEastTown, Direction.FromName(South), eastTown)

        StitchTown(southEastTown, Direction.FromName(North), eastTown)
        StitchTown(southEastTown, Direction.FromName(West), southTown)

        CreateChurchEntrance()
    End Sub

    Private Sub CreateChurchEntrance()
        Dim townLocation = RNG.FromEnumerable(Location.FromLocationType(OldLocationType.Town))
        Dim entrance = Location.Create(OldLocationType.ChurchEntrance)
        Dim direction = RNG.FromEnumerable(AllDirections.Where(Function(x) x.IsCardinal AndAlso Not townLocation.HasRoute(x)))
        StitchTown(townLocation, direction, entrance)
    End Sub

    Private Sub StitchTown(fromLocation As Location, direction As Direction, toLocation As Location)
        Route.Create(fromLocation, direction, RouteType.Road, toLocation)
        Route.Create(toLocation, direction.Opposite, RouteType.Road, fromLocation)
    End Sub

    Private Sub RollUpPlayerCharacter()
        FirstRoll()
        SecondRoll()
        Play(Sfx.CharacterCreation)
    End Sub

    Private Sub SecondRoll()
        Dim dice = PlayerCharacter.GetStatistic(CharacterStatisticType.Unassigned)
        PlayerCharacter.SetStatistic(CharacterStatisticType.Unassigned, 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(SecondRollTable)
            PlayerCharacter.ChangeStatistic(statisticType, 1)
            dice -= 1
        End While
    End Sub

    ReadOnly FirstRollTable As IReadOnlyDictionary(Of CharacterStatisticType, Integer) =
        New Dictionary(Of CharacterStatisticType, Integer) From
        {
            {CharacterStatisticType.Dexterity, 1},
            {CharacterStatisticType.Influence, 1},
            {CharacterStatisticType.Strength, 1},
            {CharacterStatisticType.Unassigned, 2},
            {CharacterStatisticType.Willpower, 1}
        }

    ReadOnly SecondRollTable As IReadOnlyDictionary(Of CharacterStatisticType, Integer) =
        New Dictionary(Of CharacterStatisticType, Integer) From
        {
            {CharacterStatisticType.Dexterity, 1},
            {CharacterStatisticType.Influence, 1},
            {CharacterStatisticType.Power, 1},
            {CharacterStatisticType.Strength, 1},
            {CharacterStatisticType.Unassigned, 1},
            {CharacterStatisticType.Willpower, 1}
        }

    Private Sub FirstRoll()
        Dim dice = PlayerCharacter.GetStatistic(CharacterStatisticType.Unassigned)
        PlayerCharacter.SetStatistic(CharacterStatisticType.Unassigned, 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(FirstRollTable)
            PlayerCharacter.ChangeStatistic(statisticType, 1)
            dice -= 1
        End While
    End Sub

    Public ReadOnly Property PlayerCharacter As Character
        Get
            Return New PlayerCharacter
        End Get
    End Property
End Module
