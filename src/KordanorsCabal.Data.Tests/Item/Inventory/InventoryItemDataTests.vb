Public Class InventoryItemDataTests
    Inherits WorldDataSubobjectTests(Of IInventoryItemData)
    Sub New()
        MyBase.New(Function(x) x.InventoryItem)
    End Sub
    <Fact>
    Sub ShouldClearInventoryAssociationDataForAGivenItem()
        WithSubobject(
            Sub(store, checker, subject)
                Dim itemId = 1L
                store.Setup(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.ClearForItem(itemId)
                store.Verify(
                    Sub(x) x.Clear.ForValue(Of Long)(
                        It.IsAny(Of Action),
                        Tables.InventoryItems,
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForItemsAssociatedWithAGivenInventory()
        WithSubobject(
            Sub(store, checker, subject)
                Dim inventoryId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadItems(inventoryId).ShouldBeNull
                store.Verify(
                    function(x) x.Record.WithValues(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.InventoryItems,
                        Columns.ItemIdColumn,
                        (Columns.InventoryIdColumn, inventoryId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAssociateAnItemWithAnInventory()
        WithSubobject(
            Sub(store, checker, subject)
                Dim inventoryId = 1L
                Dim itemId = 2L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(inventoryId, itemId)
                store.Verify(
                    Sub(x) x.Replace.Entry(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.InventoryItems,
                        (Columns.InventoryIdColumn, inventoryId),
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
