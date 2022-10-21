Public Class Lore
    Inherits BaseThingie
    Implements ILore

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Shared Function FromId(worldData As IWorldData, id As Long?) As ILore
        Return If(id.HasValue, New Lore(worldData, id.Value), Nothing)
    End Function
End Class
