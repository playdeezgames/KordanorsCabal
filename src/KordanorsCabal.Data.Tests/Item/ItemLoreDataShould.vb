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
End Class
