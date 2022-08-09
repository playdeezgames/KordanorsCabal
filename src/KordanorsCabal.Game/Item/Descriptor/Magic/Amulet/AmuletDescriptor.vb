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
            MakeList(EquipSlot.Neck))
        buffedStatisticType = statisticType
    End Sub
    Public Overrides Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        Return If(statisticType = buffedStatisticType, 1, Nothing)
    End Function
End Class
