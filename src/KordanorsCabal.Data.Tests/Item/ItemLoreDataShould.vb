Imports System.Xml.Serialization

Public Class ItemLoreDataShould
    Inherits WorldDataSubobjectTests(Of IItemLoreData)

    Public Sub New()
        MyBase.New(Function(x) x.ItemLore)
    End Sub
    <Fact>
    Sub fetch_lore_associated_with_an_item()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadForItem(itemId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadValue(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemLores,
                        Columns.LoreIdColumn,
                        (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub fetch_all_assigned_lore()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAllLore().ShouldBeNull
                store.Verify(Function(x) x.Record.All(Of Long)(It.IsAny(Of Action), Tables.ItemLores, Columns.LoreIdColumn))
            End Sub)
    End Sub
    <Fact>
    Sub clear_for_item()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                store.SetupGet(Function(x) x.Clear).Returns((New Mock(Of IStoreClear)).Object)
                subject.ClearForItem(itemId)
                store.Verify(Sub(x) x.Clear.ForValue(Of Long)(It.IsAny(Of Action), Tables.ItemLores, (Columns.ItemIdColumn, itemId)))
            End Sub)
    End Sub
    <Fact>
    Sub set_for_item()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemId = 1L
                Const loreId = 2L
                store.SetupGet(Function(x) x.Replace).Returns((New Mock(Of IStoreReplace)).Object)
                subject.Write(itemId, loreId)
                store.Verify(
                    Sub(x) x.Replace.Entry(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemLores,
                        (Columns.ItemIdColumn, itemId),
                        (Columns.LoreIdColumn, loreId)))
            End Sub)
    End Sub
End Class
