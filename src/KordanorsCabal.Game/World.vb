Public Module World
    Public Sub Start()
        Store.Reset()
        CreateTown()
        CreateDungeon(Location.FromLocationType(LocationType.ChurchEntrance).First)
        CreateFeatures()
        CreatePlayer()
    End Sub

    Private Sub CreateDungeon(location As Location)
        location = CreateDungeonLevel(location, 1, ItemType.CopperKey)
        location = CreateDungeonLevel(location, 2, ItemType.SilverKey)
        location = CreateDungeonLevel(location, 3, ItemType.GoldKey)
        location = CreateDungeonLevel(location, 4, ItemType.PlatinumKey)
        location = CreateDungeonLevel(location, 5, ItemType.ElementalOrb)
    End Sub

    Const MazeColumns = 11
    Const MazeRows = 11
    ReadOnly MazeDirections As IReadOnlyDictionary(Of Direction, MazeDirection(Of Direction)) =
        New Dictionary(Of Direction, MazeDirection(Of Direction)) From
        {
            {Direction.North, New MazeDirection(Of Direction)(Direction.South, 0, -1)},
            {Direction.East, New MazeDirection(Of Direction)(Direction.West, 1, 0)},
            {Direction.South, New MazeDirection(Of Direction)(Direction.North, 0, 1)},
            {Direction.West, New MazeDirection(Of Direction)(Direction.East, -1, 0)}
        }

    Private Function CreateLocations(maze As Maze(Of Direction), dungeonLevel As Long) As IReadOnlyList(Of Location)
        Dim locations As New List(Of Location)
        For row As Long = 0 To maze.Rows - 1
            For column As Long = 0 To maze.Columns - 1
                Dim dungeonLocation = Location.Create(LocationType.Dungeon)
                dungeonLocation.SetStatistic(LocationStatisticType.DungeonLevel, dungeonLevel)
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

    Private Function CreateDungeonLevel(fromLocation As Location, dungeonLevel As Long, bossKeyType As ItemType) As Location
        Dim maze = New Maze(Of Direction)(MazeColumns, MazeRows, MazeDirections)
        maze.Generate()
        Dim locations = CreateLocations(maze, dungeonLevel)
        PopulateLocations(locations, bossKeyType)
        Dim startingLocation = RNG.FromEnumerable(locations.Where(Function(x) x.Routes.Count > 1))
        Route.Create(fromLocation, Direction.Down, RouteType.Stairs, startingLocation)
        Route.Create(startingLocation, Direction.Up, RouteType.Stairs, fromLocation)
        Return locations.Single(Function(x) x.LocationType = LocationType.DungeonBoss)
    End Function

    Private Sub PopulateLocations(locations As IReadOnlyList(Of Location), bossKeyType As ItemType)
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
        Dim bossLocation = RNG.FromEnumerable(deadEnds)
        bossLocation.LocationType = LocationType.DungeonBoss
        partitions.Add(LocationType.DungeonBoss, New List(Of Location) From {bossLocation})
        For Each itemType In itemTypes
            Dim locationTypes = itemType.SpawnLocationTypes
            If locationTypes.Any Then
                Dim spawnLocation = RNG.FromEnumerable(locations.Where(Function(x) locationTypes.Contains(x.LocationType)))
                spawnLocation.Inventory.Add(Item.Create(itemType))
            End If
        Next
    End Sub

    Private Sub CreateFeatures()
        For Each featureType In AllFeatureTypes
            CreateFeature(featureType)
        Next
    End Sub

    Private Sub CreateFeature(featureType As FeatureType)
        Dim featureLocation = RNG.FromEnumerable(Location.FromLocationType(featureType.LocationType).Where(Function(x) Not x.HasFeature))
        Feature.Create(featureType, featureLocation)
    End Sub

    Private Sub CreatePlayer()
        Dim startingLocation = Location.FromLocationType(LocationType.TownSquare).First
        Dim playerCharacter = Character.Create(CharacterType.N00b, startingLocation)
        PlayerData.Write(playerCharacter.Id, RNG.FromEnumerable(AllDirections.Where(Function(x) x.IsCardinal)), PlayerMode.Neutral)
        RollUpPlayerCharacter()
    End Sub

    Private Sub CreateTown()
        Dim centerTown = Location.Create(LocationType.TownSquare)
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

    Public ReadOnly Property PlayerCharacter As PlayerCharacter
        Get
            Return New PlayerCharacter
        End Get
    End Property
End Module
