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
End Class
