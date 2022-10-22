Imports KordanorsCabal.Data

Friend Class FinalizeCharacterProcessor
    Inherits MenuProcessor

    Private prompt As String

    Private Shared Function ApplyPoint(statisticType As ICharacterStatisticType, nextState As UIState, currentState As UIState) As UIState
        Dim player = Game.World.PlayerCharacter(WorldData)
        player.Advancement.AssignPoint(statisticType)
        Return If(player.Advancement.IsFullyAssigned, nextState, currentState)
    End Function

    Public Sub New(currentState As UIState, nextState As UIState, cancelState As UIState, prompt As String)
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Cancel", Function() cancelState),
                ($"STR: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType1), nextState, currentState)),
                ($"DEX: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType2), nextState, currentState)),
                ($"INF: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType3), nextState, currentState)),
                ($"WIL: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType4), nextState, currentState)),
                ($"POW: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType5), nextState, currentState)),
                ($"HP: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType6), nextState, currentState)),
                ($"MP: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType7), nextState, currentState)),
                ($"Mana: ?", Function() ApplyPoint(CharacterStatisticType.FromId(WorldData, CharacterStatisticType8), nextState, currentState))
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
        Dim player = Game.World.PlayerCharacter(WorldData)
        buffer.WriteTextCentered(2, $"{CharacterStatisticType.FromId(WorldData, CharacterStatisticType9).Name}: {player.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType9))}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each index In indices
            UpdateMenuItemText(index, $"{New CharacterStatisticType(WorldData, index).Name}: {player.Statistics.GetStatistic(New CharacterStatisticType(WorldData, index))}")
        Next
    End Sub

    Public Overrides Sub Initialize()
    End Sub
End Class
