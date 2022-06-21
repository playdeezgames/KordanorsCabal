Public Module World
    Public Sub Start()
        Store.Reset()
        CreateTown()
        Dim startingLocation = Location.FromLocationType(LocationType.TownSquare).First
        Dim playerCharacter = Character.Create(CharacterType.N00b, startingLocation)
        PlayerData.Write(playerCharacter.Id, RNG.FromEnumerable(AllDirections))
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

        'TODO: church entrance
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
        Dim dice = PlayerCharacter.GetStatistic(StatisticType.Unassigned)
        PlayerCharacter.SetStatistic(StatisticType.Unassigned, 0)
        While dice > 0
            Dim statisticType = RNG.FromGenerator(SecondRollTable)
            PlayerCharacter.ChangeStatistic(statisticType, 1)
            dice -= 1
        End While
    End Sub

    ReadOnly FirstRollTable As IReadOnlyDictionary(Of StatisticType, Integer) =
        New Dictionary(Of StatisticType, Integer) From
        {
            {StatisticType.Dexterity, 1},
            {StatisticType.Influence, 1},
            {StatisticType.Strength, 1},
            {StatisticType.Unassigned, 2},
            {StatisticType.Willpower, 1}
        }

    ReadOnly SecondRollTable As IReadOnlyDictionary(Of StatisticType, Integer) =
        New Dictionary(Of StatisticType, Integer) From
        {
            {StatisticType.Dexterity, 1},
            {StatisticType.Influence, 1},
            {StatisticType.Power, 1},
            {StatisticType.Strength, 1},
            {StatisticType.Unassigned, 1},
            {StatisticType.Willpower, 1}
        }

    Private Sub FirstRoll()
        Dim dice = PlayerCharacter.GetStatistic(StatisticType.Unassigned)
        PlayerCharacter.SetStatistic(StatisticType.Unassigned, 0)
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
