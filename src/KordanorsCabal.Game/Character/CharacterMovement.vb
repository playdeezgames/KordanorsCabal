Public Class CharacterMovement
    Inherits BaseThingie
    Implements ICharacterMovement

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As ICharacterMovement
        Return If(id.HasValue, New CharacterMovement(worldData, id.Value), Nothing)
    End Function
End Class
