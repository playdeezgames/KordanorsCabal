Friend Class FinalizeCharacterProcessor
    Inherits MenuProcessor

    Private Shared Function ApplyPoint(statisticType As CharacterStatisticType) As UIState
        Dim player = World.PlayerCharacter
        player.AssignPoint(statisticType)
        Return If(player.IsFullyBaked, UIState.Prolog, UIState.FinalizeCharacter)
    End Function

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Cancel", Function() UIState.TitleScreen),
                ($"{CharacterStatisticType.Strength.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Strength)),
                ($"{CharacterStatisticType.Dexterity.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Dexterity)),
                ($"{CharacterStatisticType.Influence.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Influence)),
                ($"{CharacterStatisticType.Willpower.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Willpower)),
                ($"{CharacterStatisticType.Power.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Power)),
                ($"{CharacterStatisticType.HP.Name}: ?", Function() ApplyPoint(CharacterStatisticType.HP)),
                ($"{CharacterStatisticType.MP.Name}: ?", Function() ApplyPoint(CharacterStatisticType.MP)),
                ($"{CharacterStatisticType.Mana.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Mana))
            },
            7,
            UIState.FinalizeCharacter)
    End Sub

    Private ReadOnly table As IReadOnlyDictionary(Of CharacterStatisticType, Integer) =
        New Dictionary(Of CharacterStatisticType, Integer) From
        {
            {CharacterStatisticType.Strength, 1},
            {CharacterStatisticType.Dexterity, 2},
            {CharacterStatisticType.Influence, 3},
            {CharacterStatisticType.Willpower, 4},
            {CharacterStatisticType.Power, 5},
            {CharacterStatisticType.HP, 6},
            {CharacterStatisticType.MP, 7},
            {CharacterStatisticType.Mana, 8}
        }

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, "  Finalize Character  ", True, Hue.Blue)
        Dim player = World.PlayerCharacter
        buffer.WriteTextCentered(2, $"{CharacterStatisticType.Unassigned.Name}: {player.GetStatistic(CharacterStatisticType.Unassigned)}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each entry In table
            UpdateMenuItemText(entry.Value, $"{entry.Key.Name}: {player.GetStatistic(entry.Key)}")
        Next
    End Sub
End Class
