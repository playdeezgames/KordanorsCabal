Public Class ShoppeTypeData
    Inherits BaseData
    Implements IShoppeTypeData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub

    Public Function ReadName(shoppeTypeId As Long) As String Implements IShoppeTypeData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            ShoppeTypes,
            ShoppeTypeNameColumn,
            (ShoppeTypeIdColumn, shoppeTypeId))
    End Function
End Class
