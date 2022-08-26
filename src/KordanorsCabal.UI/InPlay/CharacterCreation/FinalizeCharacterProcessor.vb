Imports KordanorsCabal.Data

Friend Class FinalizeCharacterProcessor
    Inherits MenuProcessor

    Private prompt As String

    Private Shared Function ApplyPoint(statisticType As CharacterStatisticType, nextState As UIState, currentState As UIState) As UIState
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)
        player.AssignPoint(statisticType)
        Return If(player.IsFullyAssigned, nextState, currentState)
    End Function

    Public Sub New(currentState As UIState, nextState As UIState, cancelState As UIState, prompt As String)
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Cancel", Function() cancelState),
                ($"STR: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 1L), nextState, currentState)),
                ($"DEX: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 2L), nextState, currentState)),
                ($"INF: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 3L), nextState, currentState)),
                ($"WIL: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 4L), nextState, currentState)),
                ($"POW: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 5L), nextState, currentState)),
                ($"HP: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 6L), nextState, currentState)),
                ($"MP: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 7L), nextState, currentState)),
                ($"Mana: ?", Function() ApplyPoint(CharacterStatisticType.FromId(StaticWorldData.World, 8L), nextState, currentState))
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
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)
        buffer.WriteTextCentered(2, $"{CharacterStatisticType.FromId(StaticWorldData.World, 9L).Name}: {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 9L))}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each index In indices
            UpdateMenuItemText(index, $"{New CharacterStatisticType(StaticWorldData.World, index).Name}: {player.GetStatistic(New CharacterStatisticType(StaticWorldData.World, index))}")
        Next
    End Sub
End Class
