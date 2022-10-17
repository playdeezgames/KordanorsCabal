Friend Class SpellTypeData
    Inherits BaseData
    Implements ISpellTypeData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadName(spellTypeId As Long) As String Implements ISpellTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            SpellTypes,
            SpellTypeNameColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadMaximumLevel(spellTypeId As Long) As Long? Implements ISpellTypeData.ReadMaximumLevel
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            SpellTypes,
            MaximumLevelColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadCastCheck(spellTypeId As Long) As String Implements ISpellTypeData.ReadCastCheck
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            SpellTypes,
            CastCheckColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadCast(spellTypeId As Long) As String Implements ISpellTypeData.ReadCast
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            SpellTypes,
            CastColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function
End Class
