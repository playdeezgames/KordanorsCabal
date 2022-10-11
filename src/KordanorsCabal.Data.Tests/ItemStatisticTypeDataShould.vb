Public Class ItemStatisticTypeDataShould
    Inherits WorldDataSubobjectTests(Of IItemStatisticTypeData)
    Public Sub New()
        MyBase.New(Function(x) x.ItemStatisticType)
    End Sub
    <Fact>
    Sub have_default_value()
        WithSubobject(
            Sub(store, events, subject)
                Const statisticTypeId = 1L
                subject.ReadDefaultValue(statisticTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnValue(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemStatisticTypes,
                        Columns.DefaultValueColumn,
                        (Columns.ItemStatisticTypeIdColumn, statisticTypeId)))
            End Sub)
    End Sub
End Class
