Public Class ItemTypeDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeData)
    Sub New()
        MyBase.New(Function(x) x.ItemType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheNameOfAGivenItemType()
        WithSubobject(
            Sub(store, subject)
                Dim itemTypeId = 1L
                subject.ReadName(itemTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypes,
                    Columns.ItemTypeNameColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId)))
            End Sub)
    End Sub
End Class
