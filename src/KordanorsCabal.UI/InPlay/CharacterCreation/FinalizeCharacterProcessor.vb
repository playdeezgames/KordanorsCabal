Friend Class FinalizeCharacterProcessor
    Inherits MenuProcessor

    Private prompt As String

    Private Shared Function ApplyPoint(statisticType As OldCharacterStatisticType, nextState As UIState, currentState As UIState) As UIState
        Dim player = World.PlayerCharacter
        player.AssignPoint(statisticType)
        Return If(player.IsFullyAssigned, nextState, currentState)
    End Function

    Public Sub New(currentState As UIState, nextState As UIState, cancelState As UIState, prompt As String)
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Cancel", Function() cancelState),
                ($"{OldCharacterStatisticType.Strength.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.Strength, nextState, currentState)),
                ($"{OldCharacterStatisticType.Dexterity.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.Dexterity, nextState, currentState)),
                ($"{OldCharacterStatisticType.Influence.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.Influence, nextState, currentState)),
                ($"{OldCharacterStatisticType.Willpower.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.Willpower, nextState, currentState)),
                ($"{OldCharacterStatisticType.Power.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.Power, nextState, currentState)),
                ($"{OldCharacterStatisticType.HP.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.HP, nextState, currentState)),
                ($"{OldCharacterStatisticType.MP.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.MP, nextState, currentState)),
                ($"{OldCharacterStatisticType.Mana.Name}: ?", Function() ApplyPoint(OldCharacterStatisticType.Mana, nextState, currentState))
            },
            7,
            currentState)
        Me.prompt = prompt
    End Sub

    Private ReadOnly table As IReadOnlyDictionary(Of OldCharacterStatisticType, Integer) =
        New Dictionary(Of OldCharacterStatisticType, Integer) From
        {
            {OldCharacterStatisticType.Strength, 1},
            {OldCharacterStatisticType.Dexterity, 2},
            {OldCharacterStatisticType.Influence, 3},
            {OldCharacterStatisticType.Willpower, 4},
            {OldCharacterStatisticType.Power, 5},
            {OldCharacterStatisticType.HP, 6},
            {OldCharacterStatisticType.MP, 7},
            {OldCharacterStatisticType.Mana, 8}
        }

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, prompt, True, Hue.Blue)
        Dim player = World.PlayerCharacter
        buffer.WriteTextCentered(2, $"{OldCharacterStatisticType.Unassigned.Name}: {player.GetStatistic(OldCharacterStatisticType.Unassigned)}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each entry In table
            UpdateMenuItemText(entry.Value, $"{entry.Key.Name}: {player.GetStatistic(entry.Key)}")
        Next
    End Sub
End Class
