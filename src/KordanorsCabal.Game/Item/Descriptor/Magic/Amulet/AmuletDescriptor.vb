Public Class AmuletDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly buffedStatisticType As OldCharacterStatisticType

    Public Sub New(
                  itemTypeId As Long,
                  statisticType As OldCharacterStatisticType,
                  Optional spawnLocationTypes As IReadOnlyDictionary(Of Long, HashSet(Of LocationType)) = Nothing,
                  Optional spawnCounts As IReadOnlyDictionary(Of Long, String) = Nothing)
        MyBase.New(
            itemTypeId,
            $"Amulet of {statisticType.ToNew.Abbreviation}", ,
            spawnLocationTypes,
            spawnCounts,
            MakeList(EquipSlot.FromName(Neck)),
            New Dictionary(Of OldCharacterStatisticType, Long) From {{statisticType, 1}})
        buffedStatisticType = statisticType
    End Sub
End Class
