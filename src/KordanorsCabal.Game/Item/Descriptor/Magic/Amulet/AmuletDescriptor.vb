﻿Public Class AmuletDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly buffedStatisticType As Long

    Public Sub New(
                  itemTypeId As Long,
                  statisticType As Long,
                  Optional spawnLocationTypes As IReadOnlyDictionary(Of Long, HashSet(Of LocationType)) = Nothing,
                  Optional spawnCounts As IReadOnlyDictionary(Of Long, String) = Nothing)
        MyBase.New(
            StaticWorldData.World,
            itemTypeId, ,
            spawnLocationTypes,
            spawnCounts,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 6L)),
            New Dictionary(Of Long, Long) From {{statisticType, 1}})
        buffedStatisticType = statisticType
    End Sub
End Class
