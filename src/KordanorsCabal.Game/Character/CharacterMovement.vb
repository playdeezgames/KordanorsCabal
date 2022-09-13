Public Class CharacterMovement
    Inherits BaseThingie
    Implements ICharacterMovement

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public ReadOnly Property Character As ICharacter Implements ICharacterMovement.Character
        Get
            Return Game.Character.FromId(WorldData, Id)
        End Get
    End Property

    Public Shared Function FromId(worldData As IWorldData, id As Long?) As ICharacterMovement
        Return If(id.HasValue, New CharacterMovement(worldData, id.Value), Nothing)
    End Function
    Public Function CanMoveLeft() As Boolean Implements ICharacterMovement.CanMoveLeft
        Return CanMove(Character.Direction.PreviousDirection)
    End Function
    Public Function CanMoveRight() As Boolean Implements ICharacterMovement.CanMoveRight
        Return CanMove(Character.Direction.NextDirection)
    End Function
    Public Function CanMoveBackward() As Boolean Implements ICharacterMovement.CanMoveBackward
        Return CanMove(Character.Direction.Opposite)
    End Function
    Public Function CanMove(direction As IDirection) As Boolean Implements ICharacterMovement.CanMove
        If Character.IsEncumbered Then
            Return False
        End If
        If Character.Location Is Nothing OrElse Not Character.Location.HasRoute(direction) Then
            Return False
        End If
        If Not Character.Location.Routes(direction).CanMove(Character) Then
            Return False
        End If
        If Character.Location.Routes(direction).ToLocation.RequiresMP AndAlso Character.IsDemoralized() Then
            Return False
        End If
        Return True
    End Function
End Class
