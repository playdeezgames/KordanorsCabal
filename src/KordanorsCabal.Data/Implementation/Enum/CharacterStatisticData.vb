Public Class CharacterStatisticData
    Inherits BaseData
    Implements ICharacterStatisticData
    Friend Const TableName = "CharacterStatistics"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const StatisticTypeColumn = "StatisticType"
    Friend Const StatisticValueColumn = "StatisticVolume"

    Public Sub New(store As IStore, world As WorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{StatisticTypeColumn}] INT NOT NULL,
                [{StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{StatisticTypeColumn}]),
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub
    Public Sub Write(characterId As Long, statisticType As Long, statisticValue As Long) Implements ICharacterStatisticData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (StatisticTypeColumn, statisticType),
            (StatisticValueColumn, statisticValue))
    End Sub

    Public Function Read(characterId As Long, statisticType As Long) As Long? Implements ICharacterStatisticData.Read
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            StatisticValueColumn,
            (CharacterIdColumn, characterId),
            (StatisticTypeColumn, statisticType))
    End Function

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterStatisticData.ClearForCharacter
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
