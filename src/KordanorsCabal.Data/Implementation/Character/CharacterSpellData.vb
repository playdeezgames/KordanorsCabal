Public Class CharacterSpellData
    Inherits BaseData
    Implements ICharacterSpellData
    Friend Const TableName = "CharacterSpells"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const SpellTypeIdColumn = SpellTypeData.SpellTypeIdColumn
    Friend Const SpellLevelColumn = "SpellLevel"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        CType(World.Character, CharacterData).Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{SpellTypeIdColumn}] INT NOT NULL,
                [{SpellLevelColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{SpellTypeIdColumn}]),
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Function ReadForCharacter(characterId As Long) As IEnumerable(Of Tuple(Of Long, Long)) Implements ICharacterSpellData.ReadForCharacter
        Return Store.ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (SpellTypeIdColumn, SpellLevelColumn),
            (CharacterIdColumn, characterId))
    End Function

    Public Function Read(characterId As Long, spellType As Long) As Long? Implements ICharacterSpellData.Read
        Return Store.ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            SpellLevelColumn,
            (CharacterIdColumn, characterId),
            (SpellTypeIdColumn, spellType))
    End Function

    Public Sub Write(characterId As Long, spellType As Long, spellLevel As Long) Implements ICharacterSpellData.Write
        Store.ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (SpellTypeIdColumn, spellType),
            (SpellLevelColumn, spellLevel))
    End Sub

    Public Sub ClearForCharacter(characterId As Long) Implements ICharacterSpellData.ClearForCharacter
        Store.ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Class
