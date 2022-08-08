Public Module CharacterQuestCompletionData
    Friend Const TableName = "CharacterQuestCompletions"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestColumn = "Quest"
    Friend Const CompletionsColumn = "Completions"
    Private ReadOnly Store As SPLORR.Data.Store = StaticStore.Store
    Friend Sub Initialize()
        CharacterData.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{QuestColumn}] INT NOT NULL,
                [{CompletionsColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{QuestColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub
    Sub Write(characterId As Long, quest As Long, completions As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest),
            (CompletionsColumn, completions))
    End Sub
    Function Read(characterId As Long, quest As Long) As Long?
        Return ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CompletionsColumn,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest))
    End Function

    Friend Sub ClearForCharacter(characterId As Long)
        ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Module
