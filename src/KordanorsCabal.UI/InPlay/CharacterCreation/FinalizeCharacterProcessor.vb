﻿Friend Class FinalizeCharacterProcessor
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
                ($"STR: ?", Function() ApplyPoint(CharacterStatisticType.FromName("Strength"), nextState, currentState)),
                ($"DEX: ?", Function() ApplyPoint(CharacterStatisticType.FromName("Dexterity"), nextState, currentState)),
                ($"INF: ?", Function() ApplyPoint(CharacterStatisticType.FromName("Influence"), nextState, currentState)),
                ($"WIL: ?", Function() ApplyPoint(CharacterStatisticType.FromName("Willpower"), nextState, currentState)),
                ($"POW: ?", Function() ApplyPoint(CharacterStatisticType.FromName("Power"), nextState, currentState)),
                ($"HP: ?", Function() ApplyPoint(CharacterStatisticType.FromName("HP"), nextState, currentState)),
                ($"MP: ?", Function() ApplyPoint(CharacterStatisticType.FromName("MP"), nextState, currentState)),
                ($"Mana: ?", Function() ApplyPoint(CharacterStatisticType.FromName("Mana"), nextState, currentState))
            },
            7,
            currentState)
        Me.prompt = prompt
    End Sub

    Private ReadOnly indices As IReadOnlyList(Of Integer) =
        New List(Of Integer) From
        {
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8
        }

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, prompt, True, Hue.Blue)
        Dim player = World.PlayerCharacter
        buffer.WriteTextCentered(2, $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned).Name}: {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned))}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each index In indices
            UpdateMenuItemText(index, $"{New CharacterStatisticType(index).Name}: {player.GetStatistic(New CharacterStatisticType(index))}")
        Next
    End Sub
End Class
