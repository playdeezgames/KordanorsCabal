Friend Class SpellTypeRequiredPowerData
    Inherits BaseData
    Implements ISpellTypeRequiredPowerData
    Friend Const TableName = "SpellTypeRequiredPowers"
    Friend Const SpellTypeIdColumn = SpellTypeData.SpellTypeIdColumn
    Friend Const SpellLevelColumn = "SpellLevel"
    Friend Const PowerColumn = "Power"
    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{SpellTypeIdColumn}],
                    [{SpellLevelColumn}],
                    [{PowerColumn}]) AS
                (VALUES
                    (1,0,0),
                    (1,1,1),
                    (2,0,0),
                    (2,1,0))
                SELECT 
                    [{SpellTypeIdColumn}],
                    [{SpellLevelColumn}],
                    [{PowerColumn}]
                FROM [cte];")
    End Sub
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function Read(spellTypeId As Long, level As Long) As Long? Implements ISpellTypeRequiredPowerData.Read
        Return Store.Column.ReadValue(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            PowerColumn,
            (SpellTypeIdColumn, spellTypeId),
            (SpellLevelColumn, level))
    End Function
End Class
