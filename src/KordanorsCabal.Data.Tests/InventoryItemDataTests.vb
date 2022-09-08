Public Class InventoryItemDataTests
    Inherits WorldDataSubobjectTests(Of IInventoryItemData)
    Sub New()
        MyBase.New(Function(x) x.InventoryItem)
    End Sub
    <Fact>
    Sub ShouldClearInventoryAssociationDataForAGivenItem()
        WithSubobject(
            Sub(store, subject)
                Dim itemId = 1L
                subject.ClearForItem(itemId)
                'IStore.ClearForColumnValue<long>(Action, "InventoryItems", (ItemId, 1))
                store.Verify(
                    Sub(x) x.ClearForColumnValue(Of Long)(
                        It.IsAny(Of Action),
                        Tables.InventoryItems,
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
