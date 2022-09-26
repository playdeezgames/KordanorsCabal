Public Class AmuletDescriptor
    Inherits ItemType

    Public Sub New(
                  itemTypeId As Long)
        MyBase.New(
            StaticWorldData.World,
            itemTypeId)
    End Sub
End Class
