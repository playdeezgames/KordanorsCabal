Public Class ItemTypeStatisticType
    Inherits BaseThingie
    Implements IItemTypeStatisticType

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As ItemTypeStatisticType
        Return If(id.HasValue, New ItemTypeStatisticType(worldData, id.Value), Nothing)
    End Function
End Class
