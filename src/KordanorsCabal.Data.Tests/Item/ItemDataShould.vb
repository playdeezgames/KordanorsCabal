﻿Public Class ItemDataShould
    Inherits WorldDataSubobjectTests(Of IItemData)
    Sub New()
        MyBase.New(Function(x) x.Item)
    End Sub
    <Fact>
    Sub items_can_be_removed_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.Setup(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), It.IsAny(Of String), It.IsAny(Of (String, Long))))
                subject.Clear(itemId)
                store.Verify(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), Tables.CharacterEquipSlots, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), Tables.InventoryItems, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), Tables.ItemStatistics, (Columns.ItemIdColumn, itemId)))
                store.Verify(Sub(x) x.Clear.ForValue(It.IsAny(Of Action), Tables.Items, (Columns.ItemIdColumn, itemId)))
                store.Verify(
                    Sub(x) x.Clear.ForValue(
                        It.IsAny(Of Action),
                        Tables.ItemEvents,
                        (Columns.ItemIdColumn, itemId)))
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
                Function(x) x.Create.Entry(
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
                Sub(x) x.Column.ReadValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.Items,
                    Columns.ItemTypeIdColumn,
                    (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_their_item_type_written_into_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const itemTypeId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WriteItemType(itemId, itemTypeId)
                store.Verify(
                Sub(x) x.Column.Write(
                    It.IsAny(Of Action),
                    Tables.Items,
                    (Columns.ItemTypeIdColumn, itemTypeId),
                    (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(itemId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(
                        It.IsAny(Of Action),
                        Tables.Items,
                        Columns.ItemNameColumn,
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub set_a_name()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const itemName = "two"
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.WriteName(itemId, itemName)
                store.Verify(Sub(x) x.Column.Write(It.IsAny(Of Action), Tables.Items, (Columns.ItemNameColumn, itemName), (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
End Class
