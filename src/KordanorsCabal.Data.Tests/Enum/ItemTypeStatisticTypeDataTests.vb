Public Class ItemTypeStatisticTypeDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeStatisticTypeData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeStatisticType)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheNameOfAnItemTypeStatisticType()
        WithSubobject(
            Sub(store, subject)
                Dim itemTypeStatisticTypeId = 1L
                subject.ReadName(itemTypeStatisticTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(
                    It.IsAny(Of Action),
                    Tables.ItemTypeStatisticTypes,
                    Columns.ItemTypeStatisticTypeNameColumn,
                    (Columns.ItemTypeStatisticTypeIdColumn, itemTypeStatisticTypeId)))
            End Sub)
    End Sub
End Class
