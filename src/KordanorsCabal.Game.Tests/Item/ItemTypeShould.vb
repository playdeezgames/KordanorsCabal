Public Class ItemTypeShould
    Inherits ThingieShould(Of IItemType)
    Sub New()
        MyBase.New(AddressOf ItemType.FromId)
    End Sub
    <Fact>
    Sub item_types_hold_item_type_ids()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                subject.Id.ShouldBe(itemTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_names_fetched_from_the_data_store()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_maximum_durability()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.MaximumDurability.ShouldBeNull
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 33))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_offer()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Offer.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 34))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_price()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Price.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 35))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_repair_price()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.RepairPrice.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 36))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_offer_determiner()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Dim shoppeTypeId = 2L
                worldData.Setup(Function(x) x.ItemTypeShopType.ReadForTransactionType(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasOffer(ShoppeType.FromId(worldData.Object, shoppeTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeShopType.ReadForTransactionType(itemTypeId, 1))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_price_determiner()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Dim shoppeTypeId = 2L
                worldData.Setup(Function(x) x.ItemTypeShopType.ReadForTransactionType(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasPrice(ShoppeType.FromId(worldData.Object, shoppeTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeShopType.ReadForTransactionType(itemTypeId, 2))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_can_repair_determiner()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Dim shoppeTypeId = 2L
                worldData.Setup(Function(x) x.ItemTypeShopType.ReadForTransactionType(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CanRepair(ShoppeType.FromId(worldData.Object, shoppeTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeShopType.ReadForTransactionType(itemTypeId, 3))
            End Sub)
    End Sub
    <Fact>
    Sub have_spawn_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Spawn.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_equip_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Equip.ShouldNotBeNull
            End Sub)
    End Sub
End Class
