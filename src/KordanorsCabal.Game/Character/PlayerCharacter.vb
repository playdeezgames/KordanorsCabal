Public Class PlayerCharacter
    Inherits Character

    Sub New()
        MyBase.New(PlayerData.Read().Value)
    End Sub

    ReadOnly Property IsFullyBaked As Boolean
        Get
            Return GetStatistic(CharacterStatisticType.Unassigned) = 0
        End Get
    End Property
    Public Sub AssignPoint(statisticType As CharacterStatisticType)
        If Not IsFullyBaked Then
            ChangeStatistic(statisticType, 1)
            ChangeStatistic(CharacterStatisticType.Unassigned, -1)
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

    Public ReadOnly Property CanInteract As Boolean
        Get
            Return If(Location?.Feature?.CanInteract(Me), False)
        End Get
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

    Public Sub Interact()
        If CanInteract Then
            Mode = Location.Feature.InteractionMode(Me)
        End If
    End Sub

    Public Function CanMove(direction As Direction) As Boolean
        If IsEncumbered Then
            Return False
        End If
        If Not Location.HasRoute(direction) Then
            Return False
        End If
        Return Location.Routes(direction).CanMove(Me)
    End Function

    Friend Function HasItemType(itemType As ItemType) As Boolean
        Return Inventory.Items.Any(Function(x) x.ItemType = itemType)
    End Function

    Public Sub Move(direction As Direction)
        If CanMove(direction) Then
            Location = Location.Routes(direction).Move(Me)
        End If
    End Sub
End Class
