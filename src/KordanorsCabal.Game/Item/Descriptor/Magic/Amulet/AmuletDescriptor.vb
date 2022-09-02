Public Class AmuletDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly buffedStatisticType As Long

    Public Sub New(
                  itemTypeId As Long,
                  statisticType As Long)
        MyBase.New(
            StaticWorldData.World,
            itemTypeId, ,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 6L)),
            New Dictionary(Of Long, Long) From {{statisticType, 1}})
        buffedStatisticType = statisticType
    End Sub
End Class
