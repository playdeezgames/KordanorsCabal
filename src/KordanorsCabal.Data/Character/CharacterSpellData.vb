Public Module CharacterSpellData
    Friend Const TableName = "CharacterSpells"
    Friend Const CharacterIdColumn = CharacterData.CharacterIdColumn
    Friend Const SpellTypeColumn = "SpellType"
    Friend Const SpellLevelColumn = "SpellLevel"
    Private ReadOnly Store As SPLORR.Data.Store = StaticStore.Store
    Friend Sub Initialize()
        CharacterData.Initialize()
        Store.ExecuteNonQuery(
            $"CREATE TABLE IF NOT EXISTS [{TableName}]
            (
                [{CharacterIdColumn}] INT NOT NULL,
                [{SpellTypeColumn}] INT NOT NULL,
                [{SpellLevelColumn}] INT NOT NULL,
                UNIQUE([{CharacterIdColumn}],[{SpellTypeColumn}]),
                FOREIGN KEY ([{CharacterIdColumn}]) REFERENCES [{CharacterData.TableName}]([{CharacterData.CharacterIdColumn}])
            );")
    End Sub

    Public Function ReadForCharacter(characterId As Long) As IEnumerable(Of Tuple(Of Long, Long))
        Return ReadRecordsWithColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            (SpellTypeColumn, SpellLevelColumn),
            (CharacterIdColumn, characterId))
    End Function

    Public Function Read(characterId As Long, spellType As Long) As Long?
        Return ReadColumnValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            SpellLevelColumn,
            (CharacterIdColumn, characterId),
            (SpellTypeColumn, spellType))
    End Function

    Public Sub Write(characterId As Long, spellType As Long, spellLevel As Long)
        ReplaceRecord(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId),
            (SpellTypeColumn, spellType),
            (SpellLevelColumn, spellLevel))
    End Sub

    Friend Sub ClearForCharacter(characterId As Long)
        ClearForColumnValue(
            AddressOf Initialize,
            TableName,
            (CharacterIdColumn, characterId))
    End Sub
End Module
