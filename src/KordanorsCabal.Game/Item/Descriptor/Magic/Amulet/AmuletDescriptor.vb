Public Class AmuletDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly buffedStatisticType As CharacterStatisticType

    Public Sub New(
                  itemTypeId As Long,
                  statisticType As CharacterStatisticType,
                  Optional spawnLocationTypes As IReadOnlyDictionary(Of Long, HashSet(Of OldLocationType)) = Nothing,
                  Optional spawnCounts As IReadOnlyDictionary(Of Long, String) = Nothing)
        MyBase.New(
            itemTypeId,
            $"Amulet of {statisticType.Abbreviation}", ,
            spawnLocationTypes,
            spawnCounts,
            MakeList(EquipSlot.FromName(Neck)),
            New Dictionary(Of CharacterStatisticType, Long) From {{statisticType, 1}})
        buffedStatisticType = statisticType
    End Sub
End Class
