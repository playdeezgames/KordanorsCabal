Public Class CharacterStatisticData
    Inherits BaseData
    Implements ICharacterStatisticData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Write(characterId As Long, statisticType As Long, statisticValue As Long) Implements ICharacterStatisticData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            CharacterStatistics,
            (CharacterIdColumn, characterId),
            (CharacterStatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, statisticValue))
    End Sub

    Public Function Read(characterId As Long, statisticType As Long) As Long? Implements ICharacterStatisticData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterStatistics,
            StatisticValueColumn,
            (CharacterIdColumn, characterId),
            (CharacterStatisticTypeIdColumn, statisticType))
    End Function

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterStatisticData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            CharacterStatistics,
            (CharacterIdColumn, characterId))
    End Sub
End Class
