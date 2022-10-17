﻿Public Class CharacterQuestCompletionData
    Inherits BaseData
    Implements ICharacterQuestCompletionData
    Friend Const TableName = "CharacterQuestCompletions"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestTypeIdColumn = QuestTypeData.QuestTypeIdColumn
    Friend Const CompletionsColumn = "Completions"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Sub Write(characterId As Long, quest As Long, completions As Long) Implements ICharacterQuestCompletionData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            TableName,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest),
            (CompletionsColumn, completions))
    End Sub
    Function Read(characterId As Long, quest As Long) As Long? Implements ICharacterQuestCompletionData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            TableName,
            CompletionsColumn,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))
    End Function

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterQuestCompletionData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
