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

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{SpellTypeIdColumn}],
                    [{SpellTypeNameColumn}],
                    [{MaximumLevelColumn}],
                    [{CastCheckColumn}],
                    [{CastColumn}]) AS
                (VALUES
                    (1,'Holy Bolt', 1, 'CharacterCanCastHolyBolt', 'CharacterCastHolyBolt'),
                    (2,'Purify', 1, 'CharacterCanCastPurify', 'CharacterCastPurify'))
                SELECT 
                    [{SpellTypeIdColumn}],
                    [{SpellTypeNameColumn}],
                    [{MaximumLevelColumn}],
                    [{CastCheckColumn}],
                    [{CastColumn}]
                FROM [cte];")
    End Sub

    Public Function ReadName(spellTypeId As Long) As String Implements ISpellTypeData.ReadName
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            SpellTypeNameColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadMaximumLevel(spellTypeId As Long) As Long? Implements ISpellTypeData.ReadMaximumLevel
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            MaximumLevelColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadCastCheck(spellTypeId As Long) As String Implements ISpellTypeData.ReadCastCheck
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            CastCheckColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function

    Public Function ReadCast(spellTypeId As Long) As String Implements ISpellTypeData.ReadCast
        Return Store.ReadColumnString(
            AddressOf Initialize,
            TableName,
            CastColumn,
            (SpellTypeIdColumn, spellTypeId))
    End Function
End Class
