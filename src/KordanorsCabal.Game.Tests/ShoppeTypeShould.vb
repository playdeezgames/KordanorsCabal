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
    <Fact>
    Sub have_offers()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemType.ReadAll())
                subject.Offers.ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemType.ReadAll())
            End Sub)
    End Sub
    <Fact>
    Sub have_prices()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemType.ReadAll())
                subject.Prices.ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemType.ReadAll())
            End Sub)
    End Sub
    <Fact>
    Sub have_repairs()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemType.ReadAll())
                subject.Repairs.ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemType.ReadAll())
            End Sub)
    End Sub
    <Fact>
    Sub have_buy_prices()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.ItemType.ReadAll())
                subject.BuyPrice(ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadAll())
            End Sub)
    End Sub
    <Fact>
    Sub have_repair_prices()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.ItemType.ReadAll())
                subject.RepairPrice(ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadAll())
            End Sub)
    End Sub
    <Fact>
    Sub have_will_buy_determiner()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.ItemType.ReadAll())
                subject.WillBuy(ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemType.ReadAll())
            End Sub)
    End Sub
End Class
