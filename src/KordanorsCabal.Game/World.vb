Public Class World
    Implements IWorld
    Private worldData As IWorldData
    Sub New(worldData As IWorldData)
        Me.worldData = worldData
    End Sub
    Private Sub CreateCellar(fromLocation As ILocation)
        Dim cellar = Location.Create(worldData, LocationType.FromId(worldData, 7L))
        Route.Create(worldData, fromLocation, Direction.FromId(worldData, 6L), RouteType.FromId(worldData, 3), cellar)
        Route.Create(worldData, cellar, Direction.FromId(worldData, 5L), RouteType.FromId(worldData, 3), fromLocation)
    End Sub
    Private Sub SpawnItem(locations As IReadOnlyList(Of ILocation), dungeonLevel As IDungeonLevel, itemType As IItemType)
        Dim locationTypes = itemType.Spawn.SpawnLocationTypes(dungeonLevel)
        If locationTypes.Any Then
            Dim spawnLocation = RNG.FromEnumerable(locations.Where(Function(x) locationTypes.Select(Function(y) y.Id).Contains(x.LocationType.Id)))
            spawnLocation.Inventory.Add(Item.Create(worldData, itemType.Id))
        End If
    End Sub
    Private Function PlaceBossLocation(deadEnds As IEnumerable(Of ILocation), routeType As IRouteType) As ILocation
        Dim bossLocation = RNG.FromEnumerable(deadEnds)
        bossLocation.LocationType = LocationType.FromId(worldData, 6L)
        Dim direction = bossLocation.Routes.Directions.First
        Dim nextLocation = bossLocation.Routes.Find(direction).ToLocation
        nextLocation.Routes.Find(direction.Opposite).RouteType = routeType
        Return bossLocation
    End Function
    Private Sub SecondRoll()
        Dim dice = PlayerCharacter().Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeUnassigned))
        PlayerCharacter().Statistics.SetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeUnassigned), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(SecondRollTable)
            PlayerCharacter().Statistics.ChangeStatistic(New CharacterStatisticType(worldData, statisticType), 1)
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


    Public ReadOnly Property PlayerCharacter As IPlayerCharacter Implements IWorld.PlayerCharacter
        Get
            Return New PlayerCharacter(worldData)
        End Get
    End Property
    Private Sub FirstRoll()
        Dim dice = PlayerCharacter().Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeUnassigned))
        PlayerCharacter().Statistics.SetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeUnassigned), 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(FirstRollTable)
            PlayerCharacter().Statistics.ChangeStatistic(New CharacterStatisticType(worldData, statisticType), 1)
            dice -= 1
        End While
    End Sub

    Public ReadOnly Property IsValid As Boolean Implements IWorld.IsValid
        Get
            Return worldData.Player.Read.HasValue
        End Get
    End Property
    Private Sub RollUpPlayerCharacter()
        FirstRoll()
        SecondRoll()
        Play(Sfx.CharacterCreation)
    End Sub

    Public Sub Start() Implements IWorld.Start
        worldData.Reset()
        CreateTown()
        CreateDungeon(Location.FromLocationType(worldData, LocationType.FromId(worldData, LocationType3)).First)
        CreateMoon()
        CreateFeatures()
        CreatePlayer()
    End Sub
    Private Sub CreateTown()
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

        StitchTown(centerTown, Direction.FromId(worldData, 1L), northTown)
        StitchTown(centerTown, Direction.FromId(worldData, 2L), eastTown)
        StitchTown(centerTown, Direction.FromId(worldData, 3L), southTown)
        StitchTown(centerTown, Direction.FromId(worldData, 4L), westTown)

        StitchTown(northWestTown, Direction.FromId(worldData, 2L), northTown)
        StitchTown(northWestTown, Direction.FromId(worldData, 3L), westTown)

        StitchTown(southWestTown, Direction.FromId(worldData, 2L), southTown)
        StitchTown(southWestTown, Direction.FromId(worldData, 1L), westTown)

        StitchTown(northEastTown, Direction.FromId(worldData, 4L), northTown)
        StitchTown(northEastTown, Direction.FromId(worldData, 3L), eastTown)

        StitchTown(southEastTown, Direction.FromId(worldData, 1L), eastTown)
        StitchTown(southEastTown, Direction.FromId(worldData, 4L), southTown)

        CreateChurchEntrance()
    End Sub
    Private Sub CreateChurchEntrance()
        Dim townLocation = RNG.FromEnumerable(Location.FromLocationType(worldData, LocationType.FromId(worldData, 2L)))
        Dim entrance = Location.Create(worldData, LocationType.FromId(worldData, 3L))
        Dim direction = RNG.FromEnumerable(AllDirections(worldData).Where(Function(x) x.IsCardinal AndAlso Not townLocation.Routes.Exists(x)))
        StitchTown(townLocation, direction, entrance)
    End Sub
    Private Sub StitchTown(fromLocation As ILocation, direction As IDirection, toLocation As ILocation)
        Route.Create(worldData, fromLocation, direction, RouteType.FromId(worldData, 1L), toLocation)
        Route.Create(worldData, toLocation, direction.Opposite, RouteType.FromId(worldData, 1L), fromLocation)
    End Sub
    Private Sub CreateDungeon(location As ILocation)
        location = CreateDungeonLevel(location, DungeonLevel.FromId(worldData, 1L), 2L, RouteType.FromId(worldData, 5L)) 'TODO: add "reward item type" and "boss character type"
        location = CreateDungeonLevel(location, DungeonLevel.FromId(worldData, 2L), 3L, RouteType.FromId(worldData, 6L))
        location = CreateDungeonLevel(location, DungeonLevel.FromId(worldData, 3L), 4L, RouteType.FromId(worldData, 7L))
        location = CreateDungeonLevel(location, DungeonLevel.FromId(worldData, 4L), 5L, RouteType.FromId(worldData, 8L))
        location = CreateDungeonLevel(location, DungeonLevel.FromId(worldData, 5L), 6L, RouteType.FromId(worldData, 9L))
    End Sub
    Private Function CreateDungeonLevel(fromLocation As ILocation, dungeonLevel As IDungeonLevel, bossKeyType As Long, bossRouteType As IRouteType) As ILocation
        Dim start = DateTimeOffset.Now
        Dim elapsed = DateTimeOffset.Now - start
        Dim maze = New Maze(Of Long)(MazeColumns, MazeRows, MazeDirections)
        maze.Generate()
        Dim locations = CreateLocations(maze, dungeonLevel)
        PopulateLocations(locations, bossKeyType, bossRouteType, dungeonLevel)
        Dim startingLocation = RNG.FromEnumerable(locations.Where(Function(x) x.Routes.Count > 1))
        Route.Create(worldData, fromLocation, Direction.FromId(worldData, 6L), RouteType.FromId(worldData, 3), startingLocation)
        Route.Create(worldData, startingLocation, Direction.FromId(worldData, 5L), RouteType.FromId(worldData, 3), fromLocation)
        PopulateCharacters(locations, dungeonLevel)
        Return locations.Single(Function(x) x.LocationType.Id = 6L)
    End Function
    Private Sub CreateMoon()
        Dim locations As New List(Of ILocation)
        For row As Long = 0 To MoonRows - 1
            For column As Long = 0 To MoonColumns - 1
                Dim dungeonLocation = Location.Create(worldData, LocationType.FromId(worldData, 8L))
                dungeonLocation.DungeonLevel = DungeonLevel.FromId(worldData, 6L)
                dungeonLocation.Statistics.SetStatistic(LocationStatisticType.FromId(worldData, StatisticTypeDungeonColumn), column)
                dungeonLocation.Statistics.SetStatistic(LocationStatisticType.FromId(worldData, StatisticTypeDungeonRow), row)
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
                Route.Create(worldData, moonLocation, north, RouteType.FromId(worldData, 11L), northLocation)
                Route.Create(worldData, moonLocation, south, RouteType.FromId(worldData, 11L), southLocation)
                Route.Create(worldData, moonLocation, east, RouteType.FromId(worldData, 11L), eastLocation)
                Route.Create(worldData, moonLocation, west, RouteType.FromId(worldData, 11L), westLocation)
            Next
        Next
        PopulateCharacters(locations, DungeonLevel.FromId(worldData, 6L))
        PopulateItems(locations, DungeonLevel.FromId(worldData, 6L))
    End Sub
    Private Sub CreateFeatures()
        For Each featureType In AllFeatureTypes(worldData)
            CreateFeature(featureType)
        Next
    End Sub
    Private Sub CreatePlayer()
        Dim startingLocation = Location.FromLocationType(worldData, LocationType.FromId(worldData, 1L)).First
        Dim playerCharacter = Character.Create(worldData, CharacterType.FromId(worldData, Constants.CharacterTypes.N00b), startingLocation, CharacterType.FromId(worldData, Constants.CharacterTypes.N00b).Spawning.InitialStatistics())
        playerCharacter.Movement.Location = startingLocation 'to track that this place has been visited
        worldData.Player.Write(playerCharacter.Id, RNG.FromEnumerable(CardinalDirections(worldData)).Id, Constants.PlayerModes.Neutral)
        RollUpPlayerCharacter()
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
    Private Function CreateLocations(maze As Maze(Of Long), dungeonLevel As IDungeonLevel) As IReadOnlyList(Of ILocation)
        Dim locations As New List(Of ILocation)
        For row As Long = 0 To maze.Rows - 1
            For column As Long = 0 To maze.Columns - 1
                Dim dungeonLocation = Location.Create(worldData, LocationType.FromId(worldData, 4L))
                dungeonLocation.DungeonLevel = dungeonLevel
                dungeonLocation.Statistics.SetStatistic(LocationStatisticType.FromId(worldData, StatisticTypeDungeonColumn), column)
                dungeonLocation.Statistics.SetStatistic(LocationStatisticType.FromId(worldData, StatisticTypeDungeonRow), row)
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
                        Route.Create(worldData, dungeonLocation, Game.Direction.FromId(worldData, direction), RouteType.FromId(worldData, 2L), nextLocation)
                    End If
                Next
            Next
        Next
        Return locations
    End Function
    Private Sub PopulateLocations(locations As IReadOnlyList(Of ILocation), bossKeyType As Long, bossRouteType As IRouteType, dungeonLevel As IDungeonLevel)
        Dim deadEndId = LocationType.FromId(worldData, 5L).Id
        Dim dungeonBossId = LocationType.FromId(worldData, 6L).Id
        Dim dungeonId = LocationType.FromId(worldData, 4L).Id
        Dim partitions =
            locations.GroupBy(
                Function(x) x.Routes.Count = 1).
                    ToDictionary(Of Long, List(Of ILocation))(
                        Function(x) If(x.Key, deadEndId, dungeonId),
                        Function(x) x.ToList)
        Dim deadEnds = partitions(deadEndId)
        Dim nonDeadEnds = partitions(dungeonId)
        Dim itemTypes As New List(Of Long)
        For Each deadEnd In deadEnds
            Dim direction = deadEnd.Routes.Directions.First
            Dim nextLocation = deadEnd.Routes.Find(direction).ToLocation
            Dim route = nextLocation.Routes.Find(direction.Opposite)
            route.RouteType = RouteType.FromId(worldData, 4L)
            itemTypes.Add(1L)
        Next
        itemTypes(0) = bossKeyType
        Dim bossLocation = PlaceBossLocation(deadEnds, bossRouteType)
        partitions(deadEndId).Remove(bossLocation)
        partitions.Add(dungeonBossId, New List(Of ILocation) From {bossLocation})
        For Each itemType In itemTypes
            SpawnItem(locations, dungeonLevel, Game.ItemType.FromId(worldData, itemType))
        Next
        PopulateItems(locations, dungeonLevel)
    End Sub
    Private Sub PopulateCharacters(locations As IEnumerable(Of ILocation), dungeonLevel As IDungeonLevel)
        For Each characterType In AllCharacterTypes(worldData)
            Dim characterCount = characterType.Spawning.SpawnCount(dungeonLevel)
            If characterCount = 0 Then
                Continue For
            End If
            Dim candidates = locations.Where(Function(x) characterType.Spawning.CanSpawn(x.LocationType, dungeonLevel)).ToList
            Dim initialStatistics = characterType.Spawning.InitialStatistics()
            While characterCount > 0
                Dim location = RNG.FromList(candidates)
                Character.Create(worldData, characterType, location, initialStatistics)
                characterCount -= 1
            End While
        Next
    End Sub
    Private Sub PopulateItems(locations As IReadOnlyList(Of ILocation), dungeonLevel As IDungeonLevel)
        For Each itemType In AllItemTypes(worldData)
            Dim itemCount As Long = itemType.Spawn.RollSpawnCount(dungeonLevel)
            While itemCount > 0
                itemCount -= 1
                SpawnItem(locations, dungeonLevel, itemType)
            End While
        Next
    End Sub
    Private Sub CreateFeature(featureType As FeatureType)
        Dim featureLocation = RNG.FromEnumerable(Location.FromLocationType(worldData, featureType.LocationType).Where(Function(x) x.Feature Is Nothing))
        Feature.Create(worldData, featureType, featureLocation)
        If featureType.Id = FeatureType2 Then
            CreateCellar(featureLocation)
        End If
    End Sub

    Shared Function FromWorldData(worldData As IWorldData) As IWorld
        Return If(worldData IsNot Nothing, New World(worldData), Nothing)
    End Function
End Class
Public Module StaticWorld
    Public ReadOnly Property AllItemTypes(worldData As IWorldData) As IEnumerable(Of IItemType)
        Get
            Return worldData.ItemType.ReadAll().Select(Function(x) ItemType.FromId(worldData, x))
        End Get
    End Property
    Function AllCharacterTypes(worldData As IWorldData) As IEnumerable(Of ICharacterType)
        Return worldData.CharacterType.ReadAll().Select(Function(x) CharacterType.FromId(worldData, x))
    End Function
    Public ReadOnly Property AllFeatureTypes(worldData As IWorldData) As IEnumerable(Of FeatureType)
        Get
            Return worldData.FeatureType.ReadAll().Select(Function(x) New FeatureType(worldData, x))
        End Get
    End Property
    Friend ReadOnly Property AllDungeonLevels(worldData As IWorldData) As IReadOnlyList(Of DungeonLevel)
        Get
            Return worldData.DungeonLevel.ReadAll().Select(Function(x) New DungeonLevel(worldData, x)).ToList
        End Get
    End Property
    Friend ReadOnly Property AllDirections(worldData As IWorldData) As IEnumerable(Of Direction)
        Get
            Return worldData.Direction.ReadAll.Select(Function(x) New Direction(worldData, x))
        End Get
    End Property
    Friend ReadOnly Property CardinalDirections(worldData As IWorldData) As IEnumerable(Of Direction)
        Get
            Return AllDirections(worldData).Where(Function(x) x.IsCardinal)
        End Get
    End Property
    Public ReadOnly Property QuestDescriptors(worldData As IWorldData) As IReadOnlyDictionary(Of Long, IQuestType)
        Get
            Return worldData.QuestType.ReadAll().ToDictionary(Function(x) x, Function(x) QuestType.FromId(worldData, x))
        End Get
    End Property
End Module
