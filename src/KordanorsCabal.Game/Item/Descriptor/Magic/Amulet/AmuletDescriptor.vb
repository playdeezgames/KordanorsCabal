Public Class AmuletDescriptor
    Inherits ItemTypeDescriptor
    Private ReadOnly buffedStatisticType As CharacterStatisticType

    Public Sub New(
                  statisticType As CharacterStatisticType,
                  Optional spawnLocationTypes As IReadOnlyDictionary(Of DungeonLevel, HashSet(Of LocationType)) = Nothing,
                  Optional spawnCounts As IReadOnlyDictionary(Of DungeonLevel, String) = Nothing)
        MyBase.New(
            $"Amulet of {statisticType.Abbreviation}", ,
            spawnLocationTypes,
            spawnCounts,
            MakeList(EquipSlot.Neck),
            New Dictionary(Of CharacterStatisticType, Long) From {{statisticType, 1}})
        buffedStatisticType = statisticType
    End Sub
End Class
