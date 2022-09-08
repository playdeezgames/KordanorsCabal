Public Class ItemDataTests
    Inherits WorldDataSubobjectTests(Of IItemData)
    Sub New()
        MyBase.New(Function(x) x.Item)
    End Sub
    <Fact>
    Sub ShouldClearItemDataAndAssociatedOtherDataFromTheStore()
        WithSubobject(
            Sub(store, subject)
                Dim itemId = 1L
                subject.Clear(itemId)
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterEquipSlots, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.InventoryItems, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.ItemStatistics, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.ClearForColumnValue(It.IsAny(Of Action), Tables.Items, (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
