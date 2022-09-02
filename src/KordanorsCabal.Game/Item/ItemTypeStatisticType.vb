Public Class ItemTypeStatisticType
    Inherits BaseThingie

    Public Sub New(worldData As WorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As WorldData, id As Long?) As ItemTypeStatisticType
        Return If(id.HasValue, New ItemTypeStatisticType(worldData, id.Value), Nothing)
    End Function
End Class
