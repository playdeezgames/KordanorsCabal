﻿Public Class CharacterStatisticData
    Inherits BaseData
    Implements ICharacterStatisticData
    Friend Const TableName = "CharacterStatistics"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const CharacterStatisticTypeIdColumn = CharacterStatisticTypeData.CharacterStatisticTypeIdColumn
    Friend Const StatisticValueColumn = "StatisticValue"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Write(characterId As Long, statisticType As Long, statisticValue As Long) Implements ICharacterStatisticData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            TableName,
            (CharacterIdColumn, characterId),
            (CharacterStatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, statisticValue))
    End Sub

    Public Function Read(characterId As Long, statisticType As Long) As Long? Implements ICharacterStatisticData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            StatisticValueColumn,
            (CharacterIdColumn, characterId),
            (CharacterStatisticTypeIdColumn, statisticType))
    End Function

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterStatisticData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
