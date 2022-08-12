Public Module World
    Public Sub Start()
        StaticStore.Store.Reset()
        CreateTown()
        CreateDungeon(Location.FromLocationType(LocationType.FromName(ChurchEntrance)).First)
        CreateMoon()
        CreateFeatures()
        CreatePlayer()
    End Sub

    Private Sub CreateMoon()
        Dim locations As New List(Of Location)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim dungeonLocation = Location.Create(LocationType.FromName(Moon))
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
                Dim dungeonLocation = Location.Create(LocationType.FromName(Dungeon))
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
                        dungeonLocation.LocationType = LocationType.FromName(DungeonDeadEnd)
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
        Return locations.Single(Function(x) x.LocationType = LocationType.FromName(DungeonBoss))
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
        Dim deadEndId = LocationType.FromName(DungeonDeadEnd).Id
        Dim dungeonBossId = LocationType.FromName(DungeonBoss).Id
        Dim dungeonId = LocationType.FromName(Dungeon).Id
        Dim partitions =
            locations.GroupBy(
                Function(x) x.RouteCount = 1).
                    ToDictionary(Of Long, List(Of Location))(
                        Function(x) If(x.Key, deadEndId, dungeonId),
                        Function(x) x.ToList)
        Dim deadEnds = partitions(deadEndId)
        Dim nonDeadEnds = partitions(dungeonId)
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
        partitions(deadEndId).Remove(bossLocation)
        partitions.Add(dungeonBossId, New List(Of Location) From {bossLocation})
        For Each itemType In itemTypes
            SpawnItem(locations, dungeonLevel, itemType)
        Next
        PopulateItems(locations, dungeonLevel)
    End Sub

    Private Sub SpawnItem(locations As IReadOnlyList(Of Location), dungeonLevel As DungeonLevel, itemType As ItemType)
        Dim locationTypes = itemType.SpawnLocationTypes(dungeonLevel.Id)
        If locationTypes.Any Then
            Dim spawnLocation = RNG.FromEnumerable(locations.Where(Function(x) locationTypes.Select(Function(y) y.Id).Contains(x.LocationType.Id)))
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
        bossLocation.LocationType = LocationType.FromName(DungeonBoss)
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
        Dim cellar = Location.Create(LocationType.FromName(LocationTypeUtility.Cellar))
        Route.Create(fromLocation, Direction.FromName("Down"), RouteType.Stairs, cellar)
        Route.Create(cellar, Direction.FromName("Up"), RouteType.Stairs, fromLocation)
    End Sub

    Private Sub CreatePlayer()
        Dim startingLocation = Location.FromLocationType(LocationType.FromName(TownSquare)).First
        Dim playerCharacter = Character.Create(OldCharacterType.N00b, startingLocation)
        playerCharacter.Location = startingLocation 'to track that this place has been visited
        StaticWorldData.World.Player.Write(playerCharacter.Id, RNG.FromEnumerable(CardinalDirections).Id, PlayerMode.Neutral)
        RollUpPlayerCharacter()
    End Sub

    Private Sub CreateTown()
        Dim townLocationType = LocationType.FromName(Town)
        Dim centerTown = Location.Create(LocationType.FromName(TownSquare))
        Dim northTown = Location.Create(townLocationType)
        Dim northEastTown = Location.Create(townLocationType)
        Dim eastTown = Location.Create(townLocationType)
        Dim southEastTown = Location.Create(townLocationType)
        Dim southTown = Location.Create(townLocationType)
        Dim southWestTown = Location.Create(townLocationType)
        Dim westTown = Location.Create(townLocationType)
        Dim northWestTown = Location.Create(townLocationType)

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
        Dim townLocation = RNG.FromEnumerable(Location.FromLocationType(LocationType.FromName(Town)))
        Dim entrance = Location.Create(LocationType.FromName(ChurchEntrance))
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
        Dim dice = PlayerCharacter.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned))
        PlayerCharacter.SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(SecondRollTable)
            PlayerCharacter.ChangeStatistic(New CharacterStatisticType(statisticType), 1)
            dice -= 1
        End While
    End Sub

    ReadOnly FirstRollTable As IReadOnlyDictionary(Of Long, Integer) =
        New Dictionary(Of Long, Integer) From
        {
            {2, 1},
            {3, 1},
            {1, 1},
            {9, 2},
            {4, 1}
        }

    ReadOnly SecondRollTable As IReadOnlyDictionary(Of Long, Integer) =
        New Dictionary(Of Long, Integer) From
        {
            {2, 1},
            {3, 1},
            {5, 1},
            {1, 1},
            {9, 1},
            {4, 1}
        }

    Private Sub FirstRoll()
        Dim dice = PlayerCharacter.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned))
        PlayerCharacter.SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(FirstRollTable)
            PlayerCharacter.ChangeStatistic(New CharacterStatisticType(statisticType), 1)
            dice -= 1
        End While
    End Sub

    Public ReadOnly Property PlayerCharacter As Character
        Get
            Return New PlayerCharacter
        End Get
    End Property
End Module
