Public Class ItemTypeShopTypeData
    Inherits BaseData
    Implements IItemTypeShopTypeData
    Friend Const TableName = "ItemTypeShopTypes"
    Friend Const ItemTypeIdColumn = ItemTypeData.ItemTypeIdColumn
    Friend Const ShoppeTypeIdColumn = ShoppeTypeData.ShoppeTypeIdColumn
    Friend Const TransactionTypeIdColumn = "TransactionTypeId"

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ItemTypeIdColumn}],
                    [{ShoppeTypeIdColumn}],
                    [{TransactionTypeIdColumn}]) AS
                (VALUES
                    (7,4,2),
                    (8,1,2),
                    (9,1,1),
                    (10,2,1),
                    (10,2,2),
                    (10,2,3),
                    (15,2,1),
                    (15,2,2),
                    (15,2,3),
                    (16,2,1),
                    (16,2,2),
                    (16,2,3),
                    (17,2,1),
                    (17,2,2),
                    (17,2,3),
                    (18,2,1),
                    (18,2,2),
                    (18,2,3),
                    (19,2,1),
                    (19,2,2),
                    (29,2,3),
                    (20,2,1),
                    (20,2,2),
                    (20,2,3),
                    (21,1,2),
                    (22,4,2),
                    (23,1,2),
                    (24,3,2),
                    (25,1,2),
                    (26,3,2),
                    (27,5,2),
                    (28,5,2),
                    (29,1,2),
                    (31,1,2),
                    (33,1,2),
                    (34,1,2),
                    (36,1,2),
                    (38,1,2),
                    (39,5,2),
                    (40,1,2),
                    (41,1,2),
                    (47,1,2),
                    (52,5,2)
                )
                SELECT 
                    [{ItemTypeIdColumn}],
                    [{ShoppeTypeIdColumn}],
                    [{TransactionTypeIdColumn}]
                FROM [cte];")

    End Sub

    Public Function ReadForTransactionType(itemTypeId As Long, transationTypeId As Long) As IEnumerable(Of Long) Implements IItemTypeShopTypeData.ReadForTransactionType
        Return Store.Record.ReadRecordsWithColumnValues(Of Long, Long, Long)(
            AddressOf Initialize,
            TableName,
            ShoppeTypeIdColumn,
            (ItemTypeIdColumn, itemTypeId),
            (TransactionTypeIdColumn, transationTypeId))
    End Function
End Class
