Public Class CharacterQuestData
    Inherits BaseData
    Implements ICharacterQuestData
    Friend Const TableName = "CharacterQuests"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestTypeIdColumn = QuestTypeData.QuestTypeIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.Primitive.Execute(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{QuestTypeIdColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{QuestTypeIdColumn}]),
                FOREIGN KEY([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Sub Clear(characterId As Long, quest As Long) Implements ICharacterQuestData.Clear
        Store.Clear.ForValues(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))
    End Sub

    Public Function Read(characterId As Long, quest As Long) As Boolean Implements ICharacterQuestData.Read
        Return If(Store.Record.WithValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))?.Any, False)
    End Function

    Public Sub Write(characterId As Long, quest As Long) Implements ICharacterQuestData.Write
        Store.Replace.Entry(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterQuestData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
