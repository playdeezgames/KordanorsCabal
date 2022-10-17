Public Class CharacterQuestData
    Inherits BaseData
    Implements ICharacterQuestData
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const QuestTypeIdColumn = QuestTypeData.QuestTypeIdColumn

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Sub Clear(characterId As Long, quest As Long) Implements ICharacterQuestData.Clear
        Store.Clear.ForValues(
            AddressOf NoInitializer,
            CharacterQuests,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))
    End Sub

    Public Function Read(characterId As Long, quest As Long) As Boolean Implements ICharacterQuestData.Read
        Return If(Store.Record.WithValues(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterQuests,
            CharacterIdColumn,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))?.Any, False)
    End Function

    Public Sub Write(characterId As Long, quest As Long) Implements ICharacterQuestData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            CharacterQuests,
            (CharacterIdColumn, characterId),
            (QuestTypeIdColumn, quest))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long) Implements ICharacterQuestData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            CharacterQuests,
            (CharacterIdColumn, characterId))
    End Sub
End Class
