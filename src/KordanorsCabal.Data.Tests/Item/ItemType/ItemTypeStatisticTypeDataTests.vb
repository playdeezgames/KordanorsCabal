Public Class ItemTypeStatisticTypeDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeStatisticTypeData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeStatisticType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheNameOfAnItemTypeStatisticType()
        WithSubobject(
            Sub(store, checker, subject)
                Dim itemTypeStatisticTypeId = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(itemTypeStatisticTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(
                    It.IsAny(Of Action),
                    Tables.ItemTypeStatisticTypes,
                    Columns.ItemTypeStatisticTypeNameColumn,
                    (Columns.ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId)))
            End Sub)
    End Sub
End Class
