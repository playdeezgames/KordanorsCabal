Public Class ItemTypeShopTypeData
    Inherits BaseData
    Implements IItemTypeShopTypeData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadForTransactionType(itemTypeId As Long, transationTypeId As Long) As IEnumerable(Of Long) Implements IItemTypeShopTypeData.ReadForTransactionType
        Return Store.Record.WithValues(Of Long, Long, Long)(
            AddressOf NoInitializer,
            ItemTypeShopTypes,
            ShoppeTypeIdColumn,
            (ItemTypeIdColumn, itemTypeId),
            (TransactionTypeIdColumn, transationTypeId))
    End Function
End Class
