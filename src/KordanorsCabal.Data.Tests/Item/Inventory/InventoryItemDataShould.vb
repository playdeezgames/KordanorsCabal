Public Class InventoryItemDataShould
    Inherits WorldDataSubobjectTests(Of IInventoryItemData)
    Sub New()
        MyBase.New(Function(x) x.InventoryItem)
    End Sub
    <Fact>
    Sub Clear_Inventory_Association_Data_For_A_Given_Item()
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
    Sub Query_The_Store_For_Items_Associated_With_A_Given_Inventory()
        WithSubobject(
            Sub(store, checker, subject)
                Dim inventoryId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadItems(inventoryId).ShouldBeNull
                store.Verify(
                    Function(x) x.Record.WithValues(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.InventoryItems,
                        Columns.ItemIdColumn,
                        (Columns.InventoryIdColumn, inventoryId)))
            End Sub)
    End Sub
    <Fact>
    Sub Associate_An_Item_With_An_Inventory()
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
    <Fact>
    Sub find_inventory_associated_with_an_item()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadForItem(itemId).ShouldBeNull
                store.Verify(Function(x) x.Column.ReadValue(Of Long, Long)(
                                 It.IsAny(Of Action),
                                 Tables.InventoryItems,
                                 Columns.InventoryIdColumn,
                                 (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
