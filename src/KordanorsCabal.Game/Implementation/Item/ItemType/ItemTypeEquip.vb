Public Class ItemTypeEquip
    Inherits BaseThingie
    Implements IItemTypeEquip
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IItemTypeEquip
        Return If(id.HasValue, New ItemTypeEquip(worldData, id.Value), Nothing)
    End Function
End Class
