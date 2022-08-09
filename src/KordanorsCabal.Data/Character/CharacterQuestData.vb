Public Class CharacterQuestData
    Inherits BaseData
    Friend Const TableName = "CharacterQuests"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestColumn = "Quest"

    Public Sub New(store As Store)
        MyBase.New(store)
    End Sub

    Friend Sub Initialize()
        WorldData.Character.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{QuestColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{QuestColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Sub Clear(characterId As Long, quest As Long)
        Store.ClearForColumnValues(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest))
    End Sub

    Public Function Exists(characterId As Long, quest As Long) As Boolean
        Return Store.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest)).Any
    End Function

    Public Sub Write(characterId As Long, quest As Long)
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long)
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
