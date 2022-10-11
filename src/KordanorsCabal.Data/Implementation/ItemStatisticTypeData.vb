Public Class ItemStatisticTypeData
    Inherits BaseData
    Implements IItemStatisticTypeData
    Friend Const TableName = "ItemStatisticTypes"
    Friend Const ItemStatisticTypeIdColumn = "ItemStatisticTypeId"
    Friend Const DefaultValueColumn = "DefaultValue"
    Friend Sub Initialize()
        Store.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemStatisticTypeIdColumn}],
                    [{DefaultValueColumn}]) AS
                (VALUES
                    (1,0))
                SELECT 
                    [{ItemStatisticTypeIdColumn}],
                    [{DefaultValueColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadDefaultValue(statisticTypeId As Long) As Long? Implements IItemStatisticTypeData.ReadDefaultValue
        Return Store.ReadColumnValue(Of Long, Long)(
            AddressOf Initialize,
            TableName,
            DefaultValueColumn,
            (ItemStatisticTypeIdColumn, statisticTypeId))
    End Function
End Class
