Public Class ItemTypeTests
    Inherits BaseThingieTests(Of IItemType)
    Sub New()
        MyBase.New(AddressOf ItemType.FromId)
    End Sub
    <Fact>
    Sub item_types_hold_item_type_ids()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                subject.Id.ShouldBe(itemTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_names_fetched_from_the_data_store()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_consumption_flags_fetched_from_the_data_store()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
                subject.IsConsumed.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemType.ReadIsConsumed(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_spawn_locations_by_dungeon_level_fetched_from_the_data_store()
        Dim dungeonLevelId = 2L
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.ItemTypeSpawnLocationType).Returns((New Mock(Of IItemTypeSpawnLocationTypeData)).Object)
                Dim dungeonLevel = New DungeonLevel(worldData.Object, dungeonLevelId)
                subject.SpawnLocationTypes(dungeonLevel).ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemTypeSpawnLocationType.ReadAll(itemTypeId, dungeonLevelId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_equip_slots()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeEquipSlot.ReadForItemType(It.IsAny(Of Long)))
                subject.EquipSlots.ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemTypeEquipSlot.ReadForItemType(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_rolls_spawn_counts()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                Const dungeonLevelId = 2L
                worldData.Setup(Function(x) x.ItemTypeSpawnCount.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.RollSpawnCount(DungeonLevel.FromId(worldData.Object, dungeonLevelId)).ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeSpawnCount.Read(itemTypeId, dungeonLevelId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_attack_dice()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.AttackDice.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 2))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_encumbrance()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Encumbrance.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 1))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_maximum_damage()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.MaximumDamage.ShouldBeNull
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 3))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_defend_dice()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.DefendDice.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 4))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_maximum_durability()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.MaximumDurability.ShouldBeNull
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_offer()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Offer.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 6))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_price()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Price.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 7))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_repair_price()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.RepairPrice.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 8))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_purify_actions()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Purify.ShouldNotBeNull
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 1))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_use_actions()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Use.ShouldNotBeNull
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 3))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_can_use_actions()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CanUse.ShouldNotBeNull
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 2))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_equipped_buffs()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                Const statisticTypeId = 2L
                worldData.Setup(Function(x) x.ItemTypeCharacterStatisticBuff.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.EquippedBuff(CharacterStatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.ItemTypeCharacterStatisticBuff.Read(itemTypeId, statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_is_weapon_determiner()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.IsWeapon.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 2))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_is_armor_determiner()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.IsArmor.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 4))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_offer_determiner()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeShopType.ReadForTransactionType(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasOffer(ShoppeType.None).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeShopType.ReadForTransactionType(itemTypeId, 1))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_price_determiner()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeShopType.ReadForTransactionType(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasPrice(ShoppeType.None).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeShopType.ReadForTransactionType(itemTypeId, 2))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_can_repair_determiner()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.Setup(Function(x) x.ItemTypeShopType.ReadForTransactionType(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CanRepair(ShoppeType.None).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeShopType.ReadForTransactionType(itemTypeId, 3))
            End Sub)
    End Sub
End Class
