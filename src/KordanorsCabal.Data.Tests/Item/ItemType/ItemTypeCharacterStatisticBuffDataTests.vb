Public Class ItemTypeCharacterStatisticBuffDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeCharacterStatisticBuffData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeCharacterStatisticBuff)
    End Sub
    <Fact>
    Sub ShouldReadTheBuffForAGivenItemTypesGivenCharacterStatistic()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemTypeId = 1L
                Const characterStatisticTypeId = 2L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.Read(itemTypeId, characterStatisticTypeId)
                store.Verify(
                    Function(x) x.Column.ReadColumnValue(Of Long, Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemTypeCharacterStatisticBuffs,
                        Columns.BuffColumn,
                        (Columns.ItemTypeIdColumn, itemTypeId),
                        (Columns.CharacterStatisticTypeIdColumn, characterStatisticTypeId)))
            End Sub)
    End Sub
End Class
