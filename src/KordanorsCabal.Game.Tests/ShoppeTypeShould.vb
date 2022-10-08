Public Class ShoppeTypeShould
    Inherits ThingieShould(Of IShoppeType)
    Sub New()
        MyBase.New(AddressOf ShoppeType.FromId)
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ShoppeType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.ShoppeType.ReadName(id))
            End Sub)
    End Sub
End Class
