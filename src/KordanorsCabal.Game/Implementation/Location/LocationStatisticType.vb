Public Class LocationStatisticType
    Inherits BaseThingie
    Implements ILocationStatisticType
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ILocationStatisticType
        Return If(id.HasValue, New LocationStatisticType(worldData, id.Value), Nothing)
    End Function
End Class
