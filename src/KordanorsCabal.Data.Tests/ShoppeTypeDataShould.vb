Public Class ShoppeTypeDataShould
    Inherits WorldDataSubobjectTests(Of IShoppeTypeData)
    Sub New()
        MyBase.New(Function(x) x.ShoppeType)
    End Sub
    <Fact>
    Sub read_name_from_data_store()
        WithSubobject(
            Sub(store, events, subject)
                Const shoppeTypeId = 1L
                subject.ReadName(shoppeTypeId).ShouldBeNull
                store.Verify(
                    Function(x) x.ReadColumnString(
                        It.IsAny(Of Action),
                        Tables.ShoppeTypes,
                        Columns.ShoppeTypeNameColumn,
                        (Columns.ShoppeTypeIdColumn, shoppeTypeId)))
            End Sub)
    End Sub
End Class
