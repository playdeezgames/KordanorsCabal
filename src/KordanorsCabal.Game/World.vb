Public Module World
    Public Sub Start(worldData As WorldData)
        Dim start = DateTimeOffset.Now
        worldData.Reset()
        CreateTown(worldData)
        CreateDungeon(worldData, Location.FromLocationType(worldData, LocationType.FromId(worldData, 3L)).First)
        CreateMoon(worldData)
        CreateFeatures(worldData)
        CreatePlayer(worldData)
        Dim elapse = DateTimeOffset.Now - start
    End Sub

    Private Sub CreateMoon(worldData As WorldData)
        Dim locations As New List(Of Location)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim dungeonLocation = Location.Create(worldData, LocationType.FromId(worldData, 8L))
                dungeonLocation.DungeonLevel = DungeonLevel.FromId(worldData, 6L)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonColumn, column)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonRow, row)
                locations.Add(dungeonLocation)
            Next
        Next
        Dim north = Direction.FromId(worldData, 1L)
        Dim east = Direction.FromId(worldData, 2L)
        Dim south = Direction.FromId(worldData, 3L)
        Dim west = Direction.FromId(worldData, 4L)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim moonLocation = locations(CInt(column + row * MoonColumns))
                Dim northLocation = locations(CInt(column + ((row + MoonRows - 1) Mod MoonRows) * MoonColumns))
                Dim southLocation = locations(CInt(column + ((row + 1) Mod MoonRows) * MoonColumns))
                Dim eastLocation = locations(CInt(((column + 1) Mod MoonColumns) + row * MoonColumns))
                Dim westLocation = locations(CInt(((column + MoonColumns - 1) Mod MoonColumns) + row * MoonColumns))
                Route.Create(worldData, moonLocation, north, RouteType.MoonPath, northLocation)
                Route.Create(worldData, moonLocation, south, RouteType.MoonPath, southLocation)
                Route.Create(worldData, moonLocation, east, RouteType.MoonPath, eastLocation)
                Route.Create(worldData, moonLocation, west, RouteType.MoonPath, westLocation)
            Next
        Next
        PopulateCharacters(worldData, locations, DungeonLevel.FromId(worldData, 6L))
        PopulateItems(worldData, locations, DungeonLevel.FromId(worldData, 6L))
    End Sub

    Private Sub CreateDungeon(worldData As WorldData, location As Location)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(worldData, 1L), ItemType.CopperKey, RouteType.CopperLock) 'TODO: add "reward item type" and "boss character type"
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(worldData, 2L), ItemType.SilverKey, RouteType.SilverLock)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(worldData, 3L), ItemType.GoldKey, RouteType.GoldLock)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(worldData, 4L), ItemType.PlatinumKey, RouteType.PlatinumLock)
        location = CreateDungeonLevel(worldData, location, DungeonLevel.FromId(worldData, 5L), ItemType.ElementalOrb, RouteType.FinalLock)
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

    Public ReadOnly Property IsValid(worldData As WorldData) As Boolean
        Get
            Return worldData.Player.Read.HasValue
        End Get
    End Property

    Private Function CreateLocations(worldData As WorldData, maze As Maze(Of Long), dungeonLevel As DungeonLevel) As IReadOnlyList(Of Location)
        Dim locations As New List(Of Location)
        For row As Long = 0 To maze.Rows - 1
            For column As Long = 0 To maze.Columns - 1
                Dim dungeonLocation = Location.Create(worldData, LocationType.FromId(worldData, 4L))
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
                        dungeonLocation.LocationType = LocationType.FromId(worldData, 5L)
                    End If
                    If If(cell.GetDoor(direction)?.Open, False) Then
                        Dim nextColumn = MazeDirections(direction).DeltaX + column
                        Dim nextRow = MazeDirections(direction).DeltaY + row
                        Dim nextLocation = locations(CInt(nextColumn + nextRow * maze.Columns))
                        Route.Create(worldData, dungeonLocation, Game.Direction.FromId(worldData, direction), RouteType.Passageway, nextLocation)
                    End If
                Next
            Next
        Next
        Return locations
    End Function

    Private Function CreateDungeonLevel(worldData As WorldData, fromLocation As Location, dungeonLevel As DungeonLevel, bossKeyType As ItemType, bossRouteType As RouteType) As Location
        Dim start = DateTimeOffset.Now
        Dim elapsed = DateTimeOffset.Now - start
        Dim maze = New Maze(Of Long)(MazeColumns, MazeRows, MazeDirections)
        maze.Generate()
        Dim locations = CreateLocations(worldData, maze, dungeonLevel)
        PopulateLocations(worldData, locations, bossKeyType, bossRouteType, dungeonLevel)
        Dim startingLocation = RNG.FromEnumerable(locations.Where(Function(x) x.RouteCount > 1))
        Route.Create(worldData, fromLocation, Direction.FromId(worldData, 6L), RouteType.Stairs, startingLocation)
        Route.Create(worldData, startingLocation, Direction.FromId(worldData, 5L), RouteType.Stairs, fromLocation)
        PopulateCharacters(worldData, locations, dungeonLevel)
        Return locations.Single(Function(x) x.LocationType = LocationType.FromId(worldData, 6L))
    End Function

    Private Sub PopulateCharacters(worldData As WorldData, locations As IEnumerable(Of Location), dungeonLevel As DungeonLevel)
        For Each characterType In AllCharacterTypes(worldData)
            Dim characterCount = characterType.SpawnCount(dungeonLevel)
            If characterCount = 0 Then
                Continue For
            End If
            Dim candidates = locations.Where(Function(x) characterType.CanSpawn(x.LocationType, dungeonLevel)).ToList
            Dim initialStatistics = characterType.InitialStatistics(worldData)
            While characterCount > 0
                Dim location = RNG.FromList(candidates)
                Character.Create(worldData, characterType, location, initialStatistics)
                characterCount -= 1
            End While
        Next
    End Sub

    Private Sub PopulateLocations(worldData As WorldData, locations As IReadOnlyList(Of Location), bossKeyType As ItemType, bossRouteType As RouteType, dungeonLevel As DungeonLevel)
        Dim deadEndId = LocationType.FromId(worldData, 5L).Id
        Dim dungeonBossId = LocationType.FromId(worldData, 6L).Id
        Dim dungeonId = LocationType.FromId(worldData, 4L).Id
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
        Dim bossLocation = PlaceBossLocation(worldData, deadEnds, bossRouteType)
        partitions(deadEndId).Remove(bossLocation)
        partitions.Add(dungeonBossId, New List(Of Location) From {bossLocation})
        For Each itemType In itemTypes
            SpawnItem(worldData, locations, dungeonLevel, itemType)
        Next
        PopulateItems(worldData, locations, dungeonLevel)
    End Sub

    Private Sub SpawnItem(worldData As WorldData, locations As IReadOnlyList(Of Location), dungeonLevel As DungeonLevel, itemType As ItemType)
        Dim locationTypes = itemType.SpawnLocationTypes(dungeonLevel.Id)
        If locationTypes.Any Then
            Dim spawnLocation = RNG.FromEnumerable(locations.Where(Function(x) locationTypes.Select(Function(y) y.Id).Contains(x.LocationType.Id)))
            spawnLocation.Inventory.Add(Item.Create(worldData, itemType))
        End If
    End Sub

    Private Sub PopulateItems(worldData As WorldData, locations As IReadOnlyList(Of Location), dungeonLevel As DungeonLevel)
        For Each itemType In AllItemTypes
            Dim itemCount As Long = itemType.RollSpawnCount(dungeonLevel.Id)
            While itemCount > 0
                itemCount -= 1
                SpawnItem(worldData, locations, dungeonLevel, itemType)
            End While
        Next
    End Sub

    Private Function PlaceBossLocation(worldData As WorldData, deadEnds As IEnumerable(Of Location), routeType As RouteType) As Location
        Dim bossLocation = RNG.FromEnumerable(deadEnds)
        bossLocation.LocationType = LocationType.FromId(worldData, 6L)
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
        Dim featureLocation = RNG.FromEnumerable(Location.FromLocationType(worldData, featureType.LocationType).Where(Function(x) Not x.HasFeature))
        Feature.Create(worldData, featureType, featureLocation)
        If featureType.Id = 2L Then
            CreateCellar(worldData, featureLocation)
        End If
    End Sub

    Private Sub CreateCellar(worldData As WorldData, fromLocation As Location)
        Dim cellar = Location.Create(worldData, LocationType.FromId(worldData, 7L))
        Route.Create(worldData, fromLocation, Direction.FromId(worldData, 6L), RouteType.Stairs, cellar)
        Route.Create(worldData, cellar, Direction.FromId(worldData, 5L), RouteType.Stairs, fromLocation)
    End Sub

    Private Sub CreatePlayer(worldData As WorldData)
        Dim startingLocation = Location.FromLocationType(worldData, LocationType.FromId(worldData, 1L)).First
        Dim playerCharacter = Character.Create(worldData, CharacterType.FromId(worldData, 11), startingLocation, CharacterType.FromId(worldData, 11).InitialStatistics(worldData))
        playerCharacter.Location = startingLocation 'to track that this place has been visited
        worldData.Player.Write(playerCharacter.Id, RNG.FromEnumerable(CardinalDirections(worldData)).Id, PlayerMode.Neutral)
        RollUpPlayerCharacter(worldData)
    End Sub

    Private Sub CreateTown(worldData As WorldData)
        Dim townLocationType = LocationType.FromId(worldData, 2L)
        Dim centerTown = Location.Create(worldData, LocationType.FromId(worldData, 1L))
        Dim northTown = Location.Create(worldData, townLocationType)
        Dim northEastTown = Location.Create(worldData, townLocationType)
        Dim eastTown = Location.Create(worldData, townLocationType)
        Dim southEastTown = Location.Create(worldData, townLocationType)
        Dim southTown = Location.Create(worldData, townLocationType)
        Dim southWestTown = Location.Create(worldData, townLocationType)
        Dim westTown = Location.Create(worldData, townLocationType)
        Dim northWestTown = Location.Create(worldData, townLocationType)

        StitchTown(worldData, centerTown, Direction.FromId(worldData, 1L), northTown)
        StitchTown(worldData, centerTown, Direction.FromId(worldData, 2L), eastTown)
        StitchTown(worldData, centerTown, Direction.FromId(worldData, 3L), southTown)
        StitchTown(worldData, centerTown, Direction.FromId(worldData, 4L), westTown)

        StitchTown(worldData, northWestTown, Direction.FromId(worldData, 2L), northTown)
        StitchTown(worldData, northWestTown, Direction.FromId(worldData, 3L), westTown)

        StitchTown(worldData, southWestTown, Direction.FromId(worldData, 2L), southTown)
        StitchTown(worldData, southWestTown, Direction.FromId(worldData, 1L), westTown)

        StitchTown(worldData, northEastTown, Direction.FromId(worldData, 4L), northTown)
        StitchTown(worldData, northEastTown, Direction.FromId(worldData, 3L), eastTown)

        StitchTown(worldData, southEastTown, Direction.FromId(worldData, 1L), eastTown)
        StitchTown(worldData, southEastTown, Direction.FromId(worldData, 4L), southTown)

        CreateChurchEntrance(worldData)
    End Sub

    Private Sub CreateChurchEntrance(worldData As WorldData)
        Dim townLocation = RNG.FromEnumerable(Location.FromLocationType(worldData, LocationType.FromId(worldData, 2L)))
        Dim entrance = Location.Create(worldData, LocationType.FromId(worldData, 3L))
        Dim direction = RNG.FromEnumerable(AllDirections(worldData).Where(Function(x) x.IsCardinal AndAlso Not townLocation.HasRoute(x)))
        StitchTown(worldData, townLocation, direction, entrance)
    End Sub

    Private Sub StitchTown(worldData As WorldData, fromLocation As Location, direction As Direction, toLocation As Location)
        Route.Create(worldData, fromLocation, direction, RouteType.Road, toLocation)
        Route.Create(worldData, toLocation, direction.Opposite, RouteType.Road, fromLocation)
    End Sub

    Private Sub RollUpPlayerCharacter(worldData As WorldData)
        FirstRoll(worldData)
        SecondRoll(worldData)
        Play(Sfx.CharacterCreation)
    End Sub

    Private Sub SecondRoll(worldData As WorldData)
        Dim dice = PlayerCharacter(worldData).GetStatistic(CharacterStatisticType.FromId(worldData, 9L))
        PlayerCharacter(worldData).SetStatistic(CharacterStatisticType.FromId(worldData, 9L), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(SecondRollTable)
            PlayerCharacter(worldData).ChangeStatistic(New CharacterStatisticType(worldData, statisticType), 1)
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

    Private Sub FirstRoll(worldData As WorldData)
        Dim dice = PlayerCharacter(worldData).GetStatistic(CharacterStatisticType.FromId(worldData, 9L))
        PlayerCharacter(worldData).SetStatistic(CharacterStatisticType.FromId(worldData, 9L), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(FirstRollTable)
            PlayerCharacter(worldData).ChangeStatistic(New CharacterStatisticType(worldData, statisticType), 1)
            dice -= 1
        End While
    End Sub

    Public ReadOnly Property PlayerCharacter(worldData As WorldData) As Character
        Get
            Return New PlayerCharacter(worldData)
        End Get
    End Property
    Function AllCharacterTypes(worldData As WorldData) As IEnumerable(Of CharacterType)
        Return worldData.CharacterType.ReadAll().Select(Function(x) CharacterType.FromId(worldData, x))
    End Function
    Public ReadOnly Property AllFeatureTypes(worldData As WorldData) As IEnumerable(Of FeatureType)
        Get
            Return worldData.FeatureType.ReadAll().Select(Function(x) New FeatureType(worldData, x))
        End Get
    End Property
    Friend ReadOnly Property AllDungeonLevels(worldData As WorldData) As IReadOnlyList(Of DungeonLevel)
        Get
            Return worldData.DungeonLevel.ReadAll().Select(Function(x) New DungeonLevel(worldData, x)).ToList
        End Get
    End Property
    Friend ReadOnly Property AllDirections(worldData As WorldData) As IEnumerable(Of Direction)
        Get
            Return worldData.Direction.ReadAll.Select(Function(x) New Direction(worldData, x))
        End Get
    End Property
    Friend ReadOnly Property CardinalDirections(worldData As WorldData) As IEnumerable(Of Direction)
        Get
            Return AllDirections(worldData).Where(Function(x) x.IsCardinal)
        End Get
    End Property
End Module
