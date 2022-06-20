Friend Class FinalizeCharacterProcessor
    Inherits MenuProcessor

    Private Shared Function ApplyPoint(statisticType As StatisticType) As UIState
        Dim player = World.PlayerCharacter
        player.AssignPoint(statisticType)
        Return If(player.IsFullyBaked, UIState.Prolog, UIState.FinalizeCharacter)
    End Function

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Cancel", Function() UIState.TitleScreen),
                ($"{StatisticType.Strength.Name}: ?", Function() ApplyPoint(StatisticType.Strength)),
                ($"{StatisticType.Dexterity.Name}: ?", Function() ApplyPoint(StatisticType.Dexterity)),
                ($"{StatisticType.Influence.Name}: ?", Function() ApplyPoint(StatisticType.Influence)),
                ($"{StatisticType.Willpower.Name}: ?", Function() ApplyPoint(StatisticType.Willpower)),
                ($"{StatisticType.Power.Name}: ?", Function() ApplyPoint(StatisticType.Power)),
                ($"{StatisticType.HP.Name}: ?", Function() ApplyPoint(StatisticType.HP)),
                ($"{StatisticType.MP.Name}: ?", Function() ApplyPoint(StatisticType.MP)),
                ($"{StatisticType.Mana.Name}: ?", Function() ApplyPoint(StatisticType.Mana))
            },
            7,
            UIState.FinalizeCharacter)
    End Sub

    Private ReadOnly table As IReadOnlyDictionary(Of StatisticType, Integer) =
        New Dictionary(Of StatisticType, Integer) From
        {
            {StatisticType.Strength, 1},
            {StatisticType.Dexterity, 2},
            {StatisticType.Influence, 3},
            {StatisticType.Willpower, 4},
            {StatisticType.Power, 5},
            {StatisticType.HP, 6},
            {StatisticType.MP, 7},
            {StatisticType.Mana, 8}
        }

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, "  Finalize Character  ", True, Hue.Blue)
        Dim player = World.PlayerCharacter
        buffer.WriteTextCentered(2, $"{StatisticType.Unassigned.Name}: {player.GetStatistic(StatisticType.Unassigned)}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each entry In table
            UpdateMenuItemText(entry.Value, $"{entry.Key.Name}: {player.GetStatistic(entry.Key)}")
        Next
    End Sub
End Class
