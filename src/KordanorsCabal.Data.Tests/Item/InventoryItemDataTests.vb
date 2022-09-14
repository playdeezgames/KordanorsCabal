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
                subject.ClearForItem(itemId)
                store.Verify(
                    Sub(x) x.ClearForColumnValue(Of Long)(
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
                subject.ReadItems(inventoryId).ShouldBeNull
                store.Verify(
                    Sub(x) x.ReadRecordsWithColumnValue(Of Long, Long)(
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
                subject.Write(inventoryId, itemId)
                store.Verify(
                    Sub(x) x.ReplaceRecord(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.InventoryItems,
                        (Columns.InventoryIdColumn, inventoryId),
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
