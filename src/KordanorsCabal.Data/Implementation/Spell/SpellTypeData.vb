Friend Class SpellTypeData
    Inherits BaseData
    Implements ISpellTypeData
    Friend Const TableName = "SpellTypes"
    Friend Const SpellTypeIdColumn = "SpellTypeId"
    Friend Const SpellTypeNameColumn = "SpellTypeName"
    Friend Const MaximumLevelColumn = "MaximumLevel"
    Friend Const CastCheckColumn = "CastCheck"
    Friend Const CastColumn = "Cast"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadName(spellTypeId As Long) As String Implements ISpellTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            SpellTypeNameColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadMaximumLevel(spellTypeId As Long) As Long? Implements ISpellTypeData.ReadMaximumLevel
        Return Store.Column.ReadValue(Of Long, Long)(
            AddressOf NoInitializer,
            TableName,
            MaximumLevelColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadCastCheck(spellTypeId As Long) As String Implements ISpellTypeData.ReadCastCheck
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            CastCheckColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadCast(spellTypeId As Long) As String Implements ISpellTypeData.ReadCast
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            TableName,
            CastColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function
End Class
