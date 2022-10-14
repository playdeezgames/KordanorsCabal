Public Class CharacterQuestCompletionData
    Inherits BaseData
    Implements ICharacterQuestCompletionData
    Friend Const TableName = "CharacterQuestCompletions"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestTypeIdColumn = QuestTypeData.QuestTypeIdColumn
    Friend Const CompletionsColumn = "Completions"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        Store.Primitive.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{QuestTypeIdColumn}] INT NOT NULL,
                [{CompletionsColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{QuestTypeIdColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub
    Sub Write(characterId As Long, quest As Long, completions As Long) Implements ICharacterQuestCompletionData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest),
            (CompletionsColumn, completions))
    End Sub
    Function Read(characterId As Long, quest As Long) As Long? Implements ICharacterQuestCompletionData.Read
        Return Store.Column.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CompletionsColumn,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))
    End Function

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterQuestCompletionData.ClearForCharacter
        Store.Clear.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
