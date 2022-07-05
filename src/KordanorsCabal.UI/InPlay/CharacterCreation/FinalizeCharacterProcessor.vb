Friend Class FinalizeCharacterProcessor
    Inherits MenuProcessor

    Private prompt As String

    Private Shared Function ApplyPoint(statisticType As CharacterStatisticType, nextState As UIState, currentState As UIState) As UIState
        Dim player = World.PlayerCharacter
        player.AssignPoint(statisticType)
        Return If(player.IsFullyAssigned, nextState, currentState)
    End Function

    Public Sub New(currentState As UIState, nextState As UIState, cancelState As UIState, prompt As String)
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Cancel", Function() cancelState),
                ($"{CharacterStatisticType.Strength.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Strength, nextState, currentState)),
                ($"{CharacterStatisticType.Dexterity.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Dexterity, nextState, currentState)),
                ($"{CharacterStatisticType.Influence.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Influence, nextState, currentState)),
                ($"{CharacterStatisticType.Willpower.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Willpower, nextState, currentState)),
                ($"{CharacterStatisticType.Power.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Power, nextState, currentState)),
                ($"{CharacterStatisticType.HP.Name}: ?", Function() ApplyPoint(CharacterStatisticType.HP, nextState, currentState)),
                ($"{CharacterStatisticType.MP.Name}: ?", Function() ApplyPoint(CharacterStatisticType.MP, nextState, currentState)),
                ($"{CharacterStatisticType.Mana.Name}: ?", Function() ApplyPoint(CharacterStatisticType.Mana, nextState, currentState))
            },
            7,
            currentState)
        Me.prompt = prompt
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
        buffer.WriteTextCentered(0, prompt, True, Hue.Blue)
        Dim player = World.PlayerCharacter
        buffer.WriteTextCentered(2, $"{CharacterStatisticType.Unassigned.Name}: {player.GetStatistic(CharacterStatisticType.Unassigned)}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each entry In table
            UpdateMenuItemText(entry.Value, $"{entry.Key.Name}: {player.GetStatistic(entry.Key)}")
        Next
    End Sub
End Class
