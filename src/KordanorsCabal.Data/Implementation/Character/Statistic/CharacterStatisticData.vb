Public Class CharacterStatisticData
    Inherits BaseData
    Implements ICharacterStatisticData
    Friend Const TableName = "CharacterStatistics"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const CharacterStatisticTypeIdColumn = CharacterStatisticTypeData.CharacterStatisticTypeIdColumn
    Friend Const StatisticValueColumn = "StatisticValue"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        Store.Primitive.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{CharacterStatisticTypeIdColumn}] INT NOT NULL,
                [{StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{CharacterStatisticTypeIdColumn}]),
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub
    Public Sub Write(characterId As Long, statisticType As Long, statisticValue As Long) Implements ICharacterStatisticData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (CharacterStatisticTypeIdColumn, statisticType),
            (StatisticValueColumn, statisticValue))
    End Sub

    Public Function Read(characterId As Long, statisticType As Long) As Long? Implements ICharacterStatisticData.Read
        Return Store.Column.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            StatisticValueColumn,
            (CharacterIdColumn, characterId),
            (CharacterStatisticTypeIdColumn, statisticType))
    End Function

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterStatisticData.ClearForCharacter
        Store.Clear.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
