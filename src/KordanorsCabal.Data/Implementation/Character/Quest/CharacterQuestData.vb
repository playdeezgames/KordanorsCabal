Public Class CharacterQuestData
    Inherits BaseData
    Implements ICharacterQuestData
    Friend Const TableName = "CharacterQuests"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestColumn = "Quest"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{QuestColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{QuestColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Sub Clear(characterId As Long, quest As Long) Implements ICharacterQuestData.Clear
        Store.ClearForColumnValues(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest))
    End Sub

    Public Function Read(characterId As Long, quest As Long) As Boolean Implements ICharacterQuestData.Read
        Return If(Store.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest))?.Any, False)
    End Function

    Public Sub Write(characterId As Long, quest As Long) Implements ICharacterQuestData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestColumn, quest))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterQuestData.ClearForCharacter
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
