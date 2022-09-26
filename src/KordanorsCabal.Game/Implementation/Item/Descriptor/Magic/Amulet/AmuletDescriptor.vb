Public Class AmuletDescriptor
    Inherits ItemType

    Public Sub New(
                  itemTypeId As Long,
                  statisticType As Long)
        MyBase.New(
            StaticWorldData.World,
            itemTypeId,
            New Dictionary(Of Long, Long) From {{statisticType, 1}})
    End Sub
End Class
