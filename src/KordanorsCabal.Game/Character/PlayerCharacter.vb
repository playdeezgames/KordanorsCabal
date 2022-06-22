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
    Public Property Direction As Direction
        Get
            Return CType(PlayerData.ReadDirection().Value, Direction)
        End Get
        Set(value As Direction)
            PlayerData.WriteDirection(value)
        End Set
    End Property
    Public Property Mode As PlayerMode
        Get
            Return CType(PlayerData.ReadMode().Value, PlayerMode)
        End Get
        Set(value As PlayerMode)
            PlayerData.WriteMode(value)
        End Set
    End Property

    Public Function CanMoveLeft() As Boolean
        Return CanMove(Direction.PreviousDirection.Value)
    End Function

    Public Function CanMoveRight() As Boolean
        Return CanMove(Direction.NextDirection.Value)
    End Function

    Public Function CanMoveForward() As Boolean
        Return CanMove(Direction)
    End Function

    Public Function CanMoveBackward() As Boolean
        Return CanMove(Direction.Opposite)
    End Function

    Public Function CanMove(direction As Direction) As Boolean
        Return Location.HasRoute(direction)
    End Function

    Public Sub Move(direction As Direction)
        If CanMove(direction) Then
            Dim route = Location.Routes(direction)
            Dim toLocation = route.ToLocation
            Location = toLocation
        End If
    End Sub
End Class
