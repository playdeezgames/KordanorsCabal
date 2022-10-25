Imports KordanorsCabal.Data

Friend Class FinalizeCharacterProcessor
    Inherits MenuProcessor

    Private prompt As String

    Private Shared Function ApplyPoint(statisticType As IStatisticType, nextState As UIState, currentState As UIState) As UIState
        Dim player = World.FromWorldData(WorldData).PlayerCharacter
        player.Advancement.AssignPoint(statisticType)
        Return If(player.Advancement.IsFullyAssigned, nextState, currentState)
    End Function

    Public Sub New(currentState As UIState, nextState As UIState, cancelState As UIState, prompt As String)
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Cancel", Function() cancelState),
                ($"STR: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypeStrength), nextState, currentState)),
                ($"DEX: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypeDexterity), nextState, currentState)),
                ($"INF: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypeInfluence), nextState, currentState)),
                ($"WIL: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypeWillpower), nextState, currentState)),
                ($"POW: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypePower), nextState, currentState)),
                ($"HP: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypeHP), nextState, currentState)),
                ($"MP: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypeMP), nextState, currentState)),
                ($"Mana: ?", Function() ApplyPoint(StatisticType.FromId(WorldData, StatisticTypeMana), nextState, currentState))
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
        Dim player = World.FromWorldData(WorldData).PlayerCharacter
        buffer.WriteTextCentered(2, $"{StatisticType.FromId(WorldData, StatisticTypeUnassigned).Name}: {player.Statistics.GetStatistic(StatisticType.FromId(WorldData, StatisticTypeUnassigned))}", False, Hue.Purple)
        buffer.WriteText((0, 4), "Choose where to assignpoint(s):", False, Hue.Black)
        For Each index In indices
            UpdateMenuItemText(index, $"{New StatisticType(WorldData, index).Name}: {player.Statistics.GetStatistic(New StatisticType(WorldData, index))}")
        Next
    End Sub

    Public Overrides Sub Initialize()
    End Sub
End Class
