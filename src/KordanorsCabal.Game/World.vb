Public Module World
    Public Sub Start()
        Store.Reset()
        CreateTown()
        CreateDungeon(Location.FromLocationType(LocationType.ChurchEntrance).First)
        CreateMoon()
        CreateFeatures()
        CreatePlayer()
    End Sub

    Private Sub CreateMoon()
        Dim locations As New List(Of Location)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim dungeonLocation = Location.Create(LocationType.Moon)
                dungeonLocation.DungeonLevel = DungeonLevel.Moon
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonColumn, column)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonRow, row)
                locations.Add(dungeonLocation)
            Next
        Next
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim moonLocation = locations(CInt(column + row * MoonColumns))
                Dim northLocation = locations(CInt(column + ((row + MoonRows - 1) Mod MoonRows) * MoonColumns))
                Dim southLocation = locations(CInt(column + ((row + 1) Mod MoonRows) * MoonColumns))
                Dim eastLocation = locations(CInt(((column + 1) Mod MoonColumns) + row * MoonColumns))
                Dim westLocation = locations(CInt(((column + MoonColumns - 1) Mod MoonColumns) + row * MoonColumns))
                Route.Create(moonLocation, Direction.North, RouteType.MoonPath, northLocation)
                Route.Create(moonLocation, Direction.South, RouteType.MoonPath, southLocation)
                Route.Create(moonLocation, Direction.East, RouteType.MoonPath, eastLocation)
                Route.Create(moonLocation, Direction.West, RouteType.MoonPath, westLocation)
            Next
        Next
        PopulateCharacters(locations, DungeonLevel.Moon)
        PopulateItems(locations, DungeonLevel.Moon)
    End Sub

    Private Sub CreateDungeon(location As Location)
        location = CreateDungeonLevel(location, DungeonLevel.Level1, ItemType.CopperKey, RouteType.CopperLock) 'TODO: add "reward item type" and "boss character type"
        location = CreateDungeonLevel(location, DungeonLevel.Level2, ItemType.SilverKey, RouteType.SilverLock)
        location = CreateDungeonLevel(location, DungeonLevel.Level3, ItemType.GoldKey, RouteType.GoldLock)
        location = CreateDungeonLevel(location, DungeonLevel.Level4, ItemType.PlatinumKey, RouteType.PlatinumLock)
        location = CreateDungeonLevel(location, DungeonLevel.Level5, ItemType.ElementalOrb, RouteType.FinalLock)
    End Sub

    Const MazeColumns = 11
    Const MazeRows = 11
    Const MoonColumns = 11
    Const MoonRows = 11
    ReadOnly MazeDirections As IReadOnlyDictionary(Of Direction, MazeDirection(Of Direction)) =
        New Dictionary(Of Direction, MazeDirection(Of Direction)) From
        {
            {Direction.North, New MazeDirection(Of Direction)(Direction.South, 0, -1)},
            {Direction.East, New MazeDirection(Of Direction)(Direction.West, 1, 0)},
            {Direction.South, New MazeDirection(Of Direction)(Direction.North, 0, 1)},
            {Direction.West, New MazeDirection(Of Direction)(Direction.East, -1, 0)}
        }

    Public ReadOnly Property IsValid As Boolean
        Get
            Return PlayerData.Read.HasValue
        End Get
    End Property

    Private Function CreateLocations(maze As Maze(Of Direction), dungeonLevel As DungeonLevel) As IReadOnlyList(Of Location)
        Dim locations As New List(Of Location)
        For row As Long = 0 To maze.Rows - 1
            For column As Long = 0 To maze.Columns - 1
                Dim dungeonLocation = Location.Create(LocationType.Dungeon)
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
                        dungeonLocation.LocationType = LocationType.DungeonDeadEnd
                    End If
                    If If(cell.GetDoor(direction)?.Open, False) Then
                        Dim nextColumn = MazeDirections(direction).DeltaX + column
                        Dim nextRow = MazeDirections(direction).DeltaY + row
                        Dim nextLocation = locations(CInt(nextColumn + nextRow * maze.Columns))
                        Route.Create(dungeonLocation, direction, RouteType.Passageway, nextLocation)
                    End If
                Next
            Next
        Next
        Return locations
    End Function

    Private Function CreateDungeonLevel(fromLocation As Location, dungeonLevel As DungeonLevel, bossKeyType As ItemType, bossRouteType As RouteType) As Location
        Dim maze = New Maze(Of Direction)(MazeColumns, MazeRows, MazeDirections)
        maze.Generate()
        Dim locations = CreateLocations(maze, dungeonLevel)
        PopulateLocations(locations, bossKeyType, bossRouteType, dungeonLevel)
        Dim startingLocation = RNG.FromEnumerable(locations.Where(Function(x) x.Routes.Count > 1))
        Route.Create(fromLocation, Direction.Down, RouteType.Stairs, startingLocation)
        Route.Create(startingLocation, Direction.Up, RouteType.Stairs, fromLocation)
        PopulateCharacters(locations, dungeonLevel)
        Return locations.Single(Function(x) x.LocationType = LocationType.DungeonBoss)
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
                    ToDictionary(Of LocationType, List(Of Location))(
                        Function(x) If(x.Key, LocationType.DungeonDeadEnd, LocationType.Dungeon),
                        Function(x) x.ToList)
        Dim deadEnds = partitions(LocationType.DungeonDeadEnd)
        Dim nonDeadEnds = partitions(LocationType.Dungeon)
        Dim itemTypes As New List(Of ItemType)
        For Each deadEnd In deadEnds
            Dim direction = deadEnd.Routes.First.Key
            Dim nextLocation = deadEnd.Routes(direction).ToLocation
            Dim route = nextLocation.Routes(direction.Opposite)
            route.RouteType = RouteType.IronLock
            itemTypes.Add(ItemType.IronKey)
        Next
        itemTypes(0) = bossKeyType
        Dim bossLocation = PlaceBossLocation(deadEnds, bossRouteType)
        partitions(LocationType.DungeonDeadEnd).Remove(bossLocation)
        partitions.Add(LocationType.DungeonBoss, New List(Of Location) From {bossLocation})
        For Each itemType In itemTypes
            SpawnItem(locations, dungeonLevel, itemType)
        Next
        PopulateItems(locations, dungeonLevel)
    End Sub

    Private Sub SpawnItem(locations As IReadOnlyList(Of Location), dungeonLevel As DungeonLevel, itemType As ItemType)
        Dim locationTypes = itemType.SpawnLocationTypes(dungeonLevel)
        If locationTypes.Any Then
            Dim spawnLocation = RNG.FromEnumerable(locations.Where(Function(x) locationTypes.Contains(x.LocationType)))
            spawnLocation.Inventory.Add(Item.Create(itemType))
        End If
    End Sub

    Private Sub PopulateItems(locations As IReadOnlyList(Of Location), dungeonLevel As DungeonLevel)
        For Each itemType In AllItemTypes
            Dim itemCount As Long = itemType.RollSpawnCount(dungeonLevel)
            While itemCount > 0
                itemCount -= 1
                SpawnItem(locations, dungeonLevel, itemType)
            End While
        Next
    End Sub

    Private Function PlaceBossLocation(deadEnds As IEnumerable(Of Location), routeType As RouteType) As Location
        Dim bossLocation = RNG.FromEnumerable(deadEnds)
        bossLocation.LocationType = LocationType.DungeonBoss
        Dim direction = bossLocation.Routes.First.Key
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
        Dim cellar = Location.Create(LocationType.Cellar)
        Route.Create(fromLocation, Direction.Down, RouteType.Stairs, cellar)
        Route.Create(cellar, Direction.Up, RouteType.Stairs, fromLocation)
    End Sub

    Private Sub CreatePlayer()
        Dim startingLocation = Location.FromLocationType(LocationType.TownSquare).First
        Dim playerCharacter = Character.Create(CharacterType.N00b, startingLocation)
        playerCharacter.Location = startingLocation 'to track that this place has been visited
        PlayerData.Write(playerCharacter.Id, RNG.FromEnumerable(AllDirections.Where(Function(x) x.IsCardinal)), PlayerMode.Neutral)
        RollUpPlayerCharacter()
    End Sub

    Private Sub CreateTown()
        Dim centerTown = Location.Create(LocationType.TownSquare)

        centerTown.Inventory.Add(Item.Create(ItemType.BookOfPurify))
        centerTown.Inventory.Add(Item.Create(ItemType.RottenFood))

        Dim northTown = Location.Create(LocationType.Town)
        Dim northEastTown = Location.Create(LocationType.Town)
        Dim eastTown = Location.Create(LocationType.Town)
        Dim southEastTown = Location.Create(LocationType.Town)
        Dim southTown = Location.Create(LocationType.Town)
        Dim southWestTown = Location.Create(LocationType.Town)
        Dim westTown = Location.Create(LocationType.Town)
        Dim northWestTown = Location.Create(LocationType.Town)

        StitchTown(centerTown, Direction.North, northTown)
        StitchTown(centerTown, Direction.East, eastTown)
        StitchTown(centerTown, Direction.South, southTown)
        StitchTown(centerTown, Direction.West, westTown)

        StitchTown(northWestTown, Direction.East, northTown)
        StitchTown(northWestTown, Direction.South, westTown)

        StitchTown(southWestTown, Direction.East, southTown)
        StitchTown(southWestTown, Direction.North, westTown)

        StitchTown(northEastTown, Direction.West, northTown)
        StitchTown(northEastTown, Direction.South, eastTown)

        StitchTown(southEastTown, Direction.North, eastTown)
        StitchTown(southEastTown, Direction.West, southTown)

        CreateChurchEntrance()
    End Sub

    Private Sub CreateChurchEntrance()
        Dim townLocation = RNG.FromEnumerable(Location.FromLocationType(LocationType.Town))
        Dim entrance = Location.Create(LocationType.ChurchEntrance)
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
