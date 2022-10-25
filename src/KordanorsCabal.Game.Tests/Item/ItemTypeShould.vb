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
    Sub item_types_have_consumption_flags_fetched_from_the_data_store()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
                subject.IsConsumed.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemType.ReadIsConsumed(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_encumbrance()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Encumbrance.ShouldBeNull
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 29))
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
    Sub item_types_have_purify_actions()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Const itemId = 2L
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Purify(Item.FromId(worldData.Object, itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 1))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_use_actions()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Const characterId = 2L
                Const itemId = 3L
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Use(Character.FromId(worldData.Object, characterId), Item.FromId(worldData.Object, itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 3))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_can_use_actions()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CanUse(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 2))
                worldData.Verify(Function(x) x.Player.Read())
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
    Sub decay_items_no_event()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Const itemId = 2L
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Decay(Game.Item.FromId(worldData.Object, itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 4L))
            End Sub)
    End Sub
    <Fact>
    Sub decay_items_with_event()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Const itemId = 2L
                Const eventName = "three"
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(eventName)
                worldData.Setup(Sub(x) x.Events.Perform(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.Decay(Game.Item.FromId(worldData.Object, itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 4L))
                worldData.Verify(Sub(x) x.Events.Perform(worldData.Object, eventName, itemId))
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
    <Fact>
    Sub have_combat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Combat.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_kind()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Kind.ShouldBeNull
            End Sub)
    End Sub
End Class
