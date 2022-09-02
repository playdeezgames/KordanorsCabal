Public Class AmuletDescriptor
    Inherits ItemTypeDescriptor

    Public Sub New(
                  itemTypeId As Long,
                  statisticType As Long)
        MyBase.New(
            StaticWorldData.World,
            itemTypeId,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 6L)),
            New Dictionary(Of Long, Long) From {{statisticType, 1}})
    End Sub
End Class
