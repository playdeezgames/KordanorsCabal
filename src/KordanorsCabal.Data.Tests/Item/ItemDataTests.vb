Public Class ItemDataTests
    Inherits WorldDataSubobjectTests(Of IItemData)
    Sub New()
        MyBase.New(Function(x) x.Item)
    End Sub
    <Fact>
    Sub items_can_be_removed_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.Setup(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.Clear(itemId)
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), Tables.CharacterEquipSlots, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), Tables.InventoryItems, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), Tables.ItemStatistics, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.Clear.ClearForColumnValue(It.IsAny(Of Action), Tables.Items, (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub items_may_be_created_in_the_data_store_based_on_an_item_type()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemType = 1L
                store.SetupGet(Function(x) x.Create).Returns((New Mock(Of IStoreCreate)).Object)
                subject.Create(itemType).ShouldBe(0)
                store.Verify(
                Function(x) x.Create.CreateRecord(
                    It.IsAny(Of Action),
                    Tables.Items,
                    (Columns.ItemTypeIdColumn, itemType)))
            End Sub)
    End Sub
    <Fact>
    Sub items_can_have_their_item_type_retrieved_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadItemType(itemId).ShouldBeNull
                store.Verify(
                Sub(x) x.Column.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Items,
                    Columns.ItemTypeIdColumn,
                    (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub items_can_have_their_item_type_written_into_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const itemTypeId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WriteItemType(itemId, itemTypeId)
                store.Verify(
                Sub(x) x.Column.WriteColumnValue(
                    It.IsAny(Of Action),
                    Tables.Items,
                    (Columns.ItemTypeIdColumn, itemTypeId),
                    (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
