Public Class ShoppeTypeData
    Inherits BaseData
    Implements IShoppeTypeData
    Friend Const TableName = "ShoppeTypes"
    Friend Const ShoppeTypeIdColumn = "ShoppeTypeId"
    Friend Const ShoppeTypeNameColumn = "ShoppeTypeName"

    Friend Sub Initialize()
        Store.Primitive.ExecuteNonQuery($"CREATE TABLE IF NOT EXISTS [{TableName}] AS
                WITH [cte](
                    [{ShoppeTypeIdColumn}],
                    [{ShoppeTypeNameColumn}]) AS
                (VALUES
                    (1,'Magic'),
                    (2,'Blacksmith'),
                    (3,'Innkeeper'),
                    (4,'Healer'),
                    (5,'Black Market'))
                SELECT 
                    [{ShoppeTypeIdColumn}],
                    [{ShoppeTypeNameColumn}]
                FROM [cte];")
    End Sub

    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadName(shoppeTypeId As Long) As String Implements IShoppeTypeData.ReadName
        Return Store.Column.ReadColumnString(
            AddressOf Initialize,
            TableName,
            ShoppeTypeNameColumn,
            (ShoppeTypeIdColumn, shoppeTypeId))
    End Function
End Class
