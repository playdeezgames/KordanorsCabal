Public Class ItemTypeDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeData)
    Sub New()
        MyBase.New(Function(x) x.ItemType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheNameOfAGivenItemType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim itemTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(itemTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypes,
                    Columns.ItemTypeNameColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheIsConsumedFlagOfAGivenItemType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim itemTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadIsConsumed(itemTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadColumnValue(Of Long, Long)(
                    It.IsAny(Of Action),
                    Tables.ItemTypes,
                    Columns.IsConsumedColumn,
                    (Columns.ItemTypeIdColumn, itemTypeId)))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldReadAllItemTypesFromTheDataStore()
        WithSubobject(
            Sub(store, checker, subject)
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadAll().ShouldBeNull
                store.Verify(Function(x) x.Record.ReadRecords(Of Long)(
                                 It.IsAny(Of Action),
                                 Tables.ItemTypes,
                                 Columns.ItemTypeIdColumn))
            End Sub)
    End Sub
End Class
