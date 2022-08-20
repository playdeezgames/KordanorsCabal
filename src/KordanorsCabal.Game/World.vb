Public Module World
    Public Sub Start(worldData As WorldData)
        StaticStore.Store.Reset()
        CreateTown(worldData)
        CreateDungeon(worldData, Location.FromLocationType(LocationType.FromId(ChurchEntrance)).First)
        CreateMoon(worldData)
        CreateFeatures(worldData)
        CreatePlayer(worldData)
    End Sub

    Private Sub CreateMoon(worldData As WorldData)
        Dim locations As New List(Of Location)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim dungeonLocation = Location.Create(LocationType.FromId(Moon))
                dungeonLocation.DungeonLevel = DungeonLevel.FromId(TheMoon)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonColumn, column)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonRow, row)
                locations.Add(dungeonLocation)
            Next
        Next
        Dim north = Direction.FromId(worldData, DirectionUtility.North)
        Dim east = Direction.FromId(worldData, DirectionUtility.East)
        Dim south = Direction.FromId(worldData, DirectionUtility.South)
        Dim west = Direction.FromId(worldData, DirectionUtility.West)
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
        PopulateCharacters(locations, DungeonLevel.FromId(TheMoon))
        PopulateItems(locations, DungeonLevel.FromId(TheMoon))
    End Sub

    Private Sub CreateDungeon(worldData As WorldData, location As Location)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(Level1), ItemType.CopperKey, RouteType.CopperLock) 'TODO: add "reward item type" and "boss character type"
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(Level2), ItemType.SilverKey, RouteType.SilverLock)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(Level3), ItemType.GoldKey, RouteType.GoldLock)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(Level4), ItemType.PlatinumKey, RouteType.PlatinumLock)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(Level5), ItemType.ElementalOrb, RouteType.FinalLock)
    End Sub

    Const MazeColumns = 11
    Const MazeRows = 11
    Const MoonColumns = 11
    Const MoonRows = 11
    ReadOnly MazeDirections As IReadOnlyDictionary(Of Long, MazeDirection(Of Long)) =
        New Dictionary(Of Long, MazeDirection(Of Long)) From
        {
            {1, New MazeDirection(Of Long)(3, 0, -1)},
            {2, New MazeDirection(Of Long)(4, 1, 0)},
            {3, New MazeDirection(Of Long)(1, 0, 1)},
            {4, New MazeDirection(Of Long)(2, -1, 0)}
        }

    Public ReadOnly Property IsValid As Boolean
        Get
            Return StaticWorldData.World.Player.Read.HasValue
        End Get
    End Property

    Private Function CreateLocations(worldData As WorldData, maze As Maze(Of Long), dungeonLevel As DungeonLevel) As IReadOnlyList(Of Location)
        Dim locations As New List(Of Location)
        For row As Long = 0 To maze.Rows - 1
            For column As Long = 0 To maze.Columns - 1
                Dim dungeonLocation = Location.Create(LocationType.FromId(Dungeon))
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
                        dungeonLocation.LocationType = LocationType.FromId(DungeonDeadEnd)
                    End If
                    If If(cell.GetDoor(direction)?.Open, False) Then
                        Dim nextColumn = MazeDirections(direction).DeltaX + column
                        Dim nextRow = MazeDirections(direction).DeltaY + row
                        Dim nextLocation = locations(CInt(nextColumn + nextRow * maze.Columns))
                        Route.Create(dungeonLocation, Game.Direction.FromId(worldData, direction), RouteType.Passageway, nextLocation)
                    End If
                Next
            Next
        Next
        Return locations
    End Function

    Private Function CreateDungeonLevel(worldData As WorldData, fromLocation As Location, dungeonLevel As DungeonLevel, bossKeyType As ItemType, bossRouteType As RouteType) As Location
        Dim maze = New Maze(Of Long)(MazeColumns, MazeRows, MazeDirections)
        maze.Generate()
        Dim locations = CreateLocations(worldData, maze, dungeonLevel)
        PopulateLocations(locations, bossKeyType, bossRouteType, dungeonLevel)
        Dim startingLocation = RNG.FromEnumerable(locations.Where(Function(x) x.RouteCount > 1))
        Route.Create(fromLocation, Direction.FromId(worldData, Down), RouteType.Stairs, startingLocation)
        Route.Create(startingLocation, Direction.FromId(worldData, Up), RouteType.Stairs, fromLocation)
        PopulateCharacters(locations, dungeonLevel)
        Return locations.Single(Function(x) x.LocationType = LocationType.FromId(DungeonBoss))
    End Function

    Private Sub PopulateCharacters(locations As IEnumerable(Of Location), dungeonLevel As DungeonLevel)
        Dim start = DateTimeOffset.Now
        For Each characterType In AllCharacterTypes(StaticWorldData.World)
            Dim characterCount = characterType.SpawnCount(dungeonLevel)
            Dim candidates = locations.Where(Function(x) characterType.CanSpawn(x.LocationType, dungeonLevel))
            Dim initialStatistics = characterType.InitialStatistics(StaticWorldData.World)
            While characterCount > 0
                Dim location = RNG.FromEnumerable(candidates)
                Character.Create(StaticWorldData.World, characterType, location, initialStatistics)
                characterCount -= 1
            End While
        Next
        Dim elapsed = DateTimeOffset.Now - start
        Debug.Print($"{dungeonLevel.Name}: {elapsed}")
    End Sub

    Private Sub PopulateLocations(locations As IReadOnlyList(Of Location), bossKeyType As ItemType, bossRouteType As RouteType, dungeonLevel As DungeonLevel)
        Dim deadEndId = LocationType.FromId(DungeonDeadEnd).Id
        Dim dungeonBossId = LocationType.FromId(DungeonBoss).Id
        Dim dungeonId = LocationType.FromId(Dungeon).Id
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
        bossLocation.LocationType = LocationType.FromId(DungeonBoss)
        Dim direction = bossLocation.RouteDirections.First
        Dim nextLocation = bossLocation.Routes(direction).ToLocation
        nextLocation.Routes(direction.Opposite).RouteType = routeType
        Return bossLocation
    End Function

    Private Sub CreateFeatures(worldData As WorldData)
        For Each featureType In AllFeatureTypes(worldData)
            CreateFeature(worldData, featureType)
        Next
    End Sub

    Private Sub CreateFeature(worldData As WorldData, featureType As FeatureType)
        Dim featureLocation = RNG.FromEnumerable(Location.FromLocationType(featureType.LocationType).Where(Function(x) Not x.HasFeature))
        Feature.Create(featureType, featureLocation)
        If featureType.Id = 2L Then
            CreateCellar(worldData, featureLocation)
        End If
    End Sub

    Private Sub CreateCellar(worldData As WorldData, fromLocation As Location)
        Dim cellar = Location.Create(LocationType.FromId(LocationTypeUtility.Cellar))
        Route.Create(fromLocation, Direction.FromId(worldData, Down), RouteType.Stairs, cellar)
        Route.Create(cellar, Direction.FromId(worldData, Up), RouteType.Stairs, fromLocation)
    End Sub

    Private Sub CreatePlayer(worldData As WorldData)
        Dim startingLocation = Location.FromLocationType(LocationType.FromId(TownSquare)).First
        Dim playerCharacter = Character.Create(StaticWorldData.World, CharacterType.FromId(StaticWorldData.World, 11), startingLocation, CharacterType.FromId(StaticWorldData.World, 1).InitialStatistics(StaticWorldData.World))
        playerCharacter.Location = startingLocation 'to track that this place has been visited
        StaticWorldData.World.Player.Write(playerCharacter.Id, RNG.FromEnumerable(CardinalDirections(worldData)).Id, PlayerMode.Neutral)
        RollUpPlayerCharacter()
    End Sub

    Private Sub CreateTown(worldData As WorldData)
        Dim townLocationType = LocationType.FromId(Town)
        Dim centerTown = Location.Create(LocationType.FromId(TownSquare))
        Dim northTown = Location.Create(townLocationType)
        Dim northEastTown = Location.Create(townLocationType)
        Dim eastTown = Location.Create(townLocationType)
        Dim southEastTown = Location.Create(townLocationType)
        Dim southTown = Location.Create(townLocationType)
        Dim southWestTown = Location.Create(townLocationType)
        Dim westTown = Location.Create(townLocationType)
        Dim northWestTown = Location.Create(townLocationType)

        StitchTown(centerTown, Direction.FromId(worldData, North), northTown)
        StitchTown(centerTown, Direction.FromId(worldData, East), eastTown)
        StitchTown(centerTown, Direction.FromId(worldData, South), southTown)
        StitchTown(centerTown, Direction.FromId(worldData, West), westTown)

        StitchTown(northWestTown, Direction.FromId(worldData, East), northTown)
        StitchTown(northWestTown, Direction.FromId(worldData, South), westTown)

        StitchTown(southWestTown, Direction.FromId(worldData, East), southTown)
        StitchTown(southWestTown, Direction.FromId(worldData, North), westTown)

        StitchTown(northEastTown, Direction.FromId(worldData, West), northTown)
        StitchTown(northEastTown, Direction.FromId(worldData, South), eastTown)

        StitchTown(southEastTown, Direction.FromId(worldData, North), eastTown)
        StitchTown(southEastTown, Direction.FromId(worldData, West), southTown)

        CreateChurchEntrance(worldData)
    End Sub

    Private Sub CreateChurchEntrance(worldData As WorldData)
        Dim townLocation = RNG.FromEnumerable(Location.FromLocationType(LocationType.FromId(Town)))
        Dim entrance = Location.Create(LocationType.FromId(ChurchEntrance))
        Dim direction = RNG.FromEnumerable(AllDirections(worldData).Where(Function(x) x.IsCardinal AndAlso Not townLocation.HasRoute(x)))
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
        Dim dice = PlayerCharacter.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 9L))
        PlayerCharacter.SetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 9L), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(SecondRollTable)
            PlayerCharacter.ChangeStatistic(New CharacterStatisticType(StaticWorldData.World, statisticType), 1)
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
        Dim dice = PlayerCharacter.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 9L))
        PlayerCharacter.SetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 9L), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(FirstRollTable)
            PlayerCharacter.ChangeStatistic(New CharacterStatisticType(StaticWorldData.World, statisticType), 1)
            dice -= 1
        End While
    End Sub

    Public ReadOnly Property PlayerCharacter As Character
        Get
            Return New PlayerCharacter(StaticWorldData.World)
        End Get
    End Property
    Function AllCharacterTypes(worldData As WorldData) As IEnumerable(Of CharacterType)
        Return worldData.CharacterType.ReadAll().Select(Function(x) CharacterType.FromId(worldData, x))
    End Function
End Module
