Public Class Durability
    Inherits BaseThingie
    Implements IDurability

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IDurability
        Return If(id.HasValue, New Durability(worldData, id.Value), Nothing)
    End Function
End Class
