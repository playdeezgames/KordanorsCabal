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
                ($"STR: ?", Function() ApplyPoint(OldCharacterStatisticType.Strength, nextState, currentState)),
                ($"DEX: ?", Function() ApplyPoint(OldCharacterStatisticType.Dexterity, nextState, currentState)),
                ($"INF: ?", Function() ApplyPoint(OldCharacterStatisticType.Influence, nextState, currentState)),
                ($"WIL: ?", Function() ApplyPoint(OldCharacterStatisticType.Willpower, nextState, currentState)),
                ($"POW: ?", Function() ApplyPoint(OldCharacterStatisticType.Power, nextState, currentState)),
                ($"HP: ?", Function() ApplyPoint(OldCharacterStatisticType.HP, nextState, currentState)),
                ($"MP: ?", Function() ApplyPoint(OldCharacterStatisticType.MP, nextState, currentState)),
                ($"Mana: ?", Function() ApplyPoint(OldCharacterStatisticType.Mana, nextState, currentState))
            },
            7,
            currentState)
        Me.prompt = prompt
    End Sub

    Private ReadOnly table As IReadOnlyDictionary(Of Long, Integer) =
        New Dictionary(Of Long, Integer) From
        {
            {OldCharacterStatisticType.Strength, 1},
            {OldCharacterStatisticType.Dexterity, 2},
            {3, 3},
            {OldCharacterStatisticType.Willpower, 4},
            {OldCharacterStatisticType.Power, 5},
            {OldCharacterStatisticType.HP, 6},
            {OldCharacterStatisticType.MP, 7},
            {OldCharacterStatisticType.Mana, 8}
        }

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, prompt, True, Hue.Blue)
        Dim player = World.PlayerCharacter
        buffer.WriteTextCentered(2, $"{OldCharacterStatisticType.Unassigned.ToNew.Name}: {player.GetStatistic(OldCharacterStatisticType.Unassigned.ToNew)}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each entry In table
            UpdateMenuItemText(entry.Value, $"{New CharacterStatisticType(entry.Key).Name}: {player.GetStatistic(New CharacterStatisticType(entry.Key))}")
        Next
    End Sub
End Class
