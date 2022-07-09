Public Module CharacterQuestData
    Friend Const TableName = "ChracterQuests"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestColumn = "Quest"
    Friend Sub Initialize()
        CharacterData.Initialize()
        ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{QuestColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{QuestColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub
    Public Function Exists(characterId As Long, quest As Long) As Boolean
        Return ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest)).Any
    End Function

    Public Sub Write(characterId As Long, quest As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest))
    End Sub
End Module
