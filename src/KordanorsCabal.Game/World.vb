Public Module World
    Public Sub Start()
        Store.Reset()
        Dim startingLocation = Location.Create()
        Dim playerCharacter = Character.Create(CharacterType.N00b, startingLocation)
        PlayerData.Write(playerCharacter.Id)
        RollUpPlayerCharacter()
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
