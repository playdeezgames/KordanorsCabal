Public Class CharacterSpellData
    Inherits BaseData
    Implements ICharacterSpellData
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const SpellTypeIdColumn = SpellTypeData.SpellTypeIdColumn
    Friend Const SpellLevelColumn = "SpellLevel"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadForCharacter(characterId As Long) As IEnumerable(Of Tuple(Of Long, Long)) Implements ICharacterSpellData.ReadForCharacter
        Return Store.Record.WithValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterSpells,
            (SpellTypeIdColumn, SpellLevelColumn),
            (CharacterIdColumn, characterId))
    End Function

    Public Function Read(characterId As Long, spellType As Long) As Long? Implements ICharacterSpellData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf NoInitializer,
            CharacterSpells,
            SpellLevelColumn,
            (CharacterIdColumn, characterId),
            (SpellTypeIdColumn, spellType))
    End Function

    Public Sub Write(characterId As Long, spellType As Long, spellLevel As Long) Implements ICharacterSpellData.Write
        Store.Replace.Entry(
            AddressOf NoInitializer,
            CharacterSpells,
            (CharacterIdColumn, characterId),
            (SpellTypeIdColumn, spellType),
            (SpellLevelColumn, spellLevel))
    End Sub

    Public Sub ClearForCharacter(characterId As Long) Implements ICharacterSpellData.ClearForCharacter
        Store.Clear.ForValue(
            AddressOf NoInitializer,
            CharacterSpells,
            (CharacterIdColumn, characterId))
    End Sub
End Class
