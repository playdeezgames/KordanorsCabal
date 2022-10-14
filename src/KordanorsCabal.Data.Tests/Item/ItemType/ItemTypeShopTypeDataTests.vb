Public Class ItemTypeShopTypeDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeShopTypeData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeShopType)
    End Sub
    <Fact>
    Sub ShouldGiveAListOfShopTypesForAGivenItemTypeAndTransationType()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemTypeId = 1L
                Const transactionTypeId = 2L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadForTransactionType(itemTypeId, transactionTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.ReadRecordsWithColumnValues(Of Long, Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemTypeShopTypes,
                        Columns.ShoppeTypeIdColumn,
                        (Columns.ItemTypeIdColumn, itemTypeId),
                        (Columns.TransactionTypeIdColumn, transactionTypeId)))
            End Sub)
    End Sub
End Class
