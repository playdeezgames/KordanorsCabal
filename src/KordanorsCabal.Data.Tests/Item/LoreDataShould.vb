Imports SQLitePCL

Public Class LoreDataShould
    Inherits WorldDataSubobjectTests(Of ILoreData)

    Public Sub New()
        MyBase.New(Function(x) x.Lore)
    End Sub
    <Fact>
    Sub read_all_lore_ids()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll().ShouldBeNull
                store.Verify(
                    Function(x) x.Record.All(Of Long)(
                        It.IsAny(Of Action),
                        Tables.Lores,
                        Columns.LoreIdColumn))
            End Sub)
    End Sub
    <Fact>
    Sub read_text()
        WithSubobject(
            Sub(store, checker, subject)
                Const loreId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadText(loreId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(
                        It.IsAny(Of Action),
                        Tables.Lores,
                        Columns.LoreTextColumn,
                        (Columns.LoreIdColumn, loreId)))
            End Sub)
    End Sub
    <Fact>
    Sub read_item_name()
        WithSubobject(
            Sub(store, checker, subject)
                Const loreId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadItemName(loreId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(
                        It.IsAny(Of Action),
                        Tables.Lores,
                        Columns.ItemNameColumn,
                        (Columns.LoreIdColumn, loreId)))
            End Sub)
    End Sub
End Class
