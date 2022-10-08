Public Class Equipment
    Inherits BaseThingie
    Implements IEquipment

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IEquipment
        Return If(id.HasValue, New Equipment(worldData, id.Value), Nothing)
    End Function
End Class
