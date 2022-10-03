Public Class ItemTypeShopTypeDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeShopTypeData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeShopType)
    End Sub
    <Fact>
    Sub ShouldGiveAListOfShopTypesForAGivenItemTypeAndTransationType()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemTypeId = 1L
                Const transactionTypeId = 2L
                subject.ReadForTransactionType(itemTypeId, transactionTypeId).ShouldBeEmpty
            End Sub)
    End Sub
End Class
