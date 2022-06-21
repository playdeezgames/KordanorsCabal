Public Class PlayerCharacter
    Inherits Character

    Sub New()
        MyBase.New(PlayerData.Read().Value)
    End Sub

    ReadOnly Property IsFullyBaked As Boolean
        Get
            Return GetStatistic(StatisticType.Unassigned) = 0
        End Get
    End Property
    Public Sub AssignPoint(statisticType As StatisticType)
        If Not IsFullyBaked Then
            ChangeStatistic(statisticType, 1)
            ChangeStatistic(StatisticType.Unassigned, -1)
        End If
    End Sub
    Public ReadOnly Property Direction As Direction
        Get
            Return CType(PlayerData.ReadDirection().Value, Direction)
        End Get
    End Property
End Class
