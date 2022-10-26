Imports SQLitePCL

Public Class ItemShould
    Inherits ThingieShould(Of IItem)
    Sub New()
        MyBase.New(AddressOf Item.FromId)
    End Sub
    <Fact>
    Sub create_from_item_type()
        Const itemTypeId = 1L
        Dim worldData As New Mock(Of IWorldData)
        worldData.Setup(Function(x) x.Item.Create(It.IsAny(Of Long)))
        Item.Create(worldData.Object, ItemType.FromId(worldData.Object, itemTypeId))
        worldData.Verify(Function(x) x.Item.Create(itemTypeId))
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub have_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemType.ReadName(It.IsAny(Of Long)))
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.Item.ReadName(id))
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_item_type()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long)))
                subject.ItemType.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
            End Sub)
    End Sub
    <Fact>
    Sub purify()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Purify()
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub allow_destruction()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Sub(x) x.Item.Clear(It.IsAny(Of Long)))
                subject.Destroy()
                worldData.Verify(Sub(x) x.Item.Clear(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_weapon_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Weapon.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_durability_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Durability.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_repair_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Repair.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_armor_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Armor.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_equipment_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Equipment.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_usage_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Usage.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub decay()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                subject.Decay()
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 4L))
            End Sub)
    End Sub
    <Fact>
    Sub have_an_inventory()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.InventoryItem.ReadForItem(It.IsAny(Of Long)))
                subject.Inventory.ShouldBeNull
                worldData.Verify(Function(x) x.InventoryItem.ReadForItem(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_lore()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemLore.ReadForItem(It.IsAny(Of Long)))
                subject.Lore.ShouldBeNull
                worldData.Verify(Function(x) x.ItemLore.ReadForItem(id))
            End Sub)
    End Sub
    <Fact>
    Sub assign_lore()
        WithSubject(
            Sub(worldData, id, subject)
                Const loreId = 2L
                worldData.Setup(Sub(x) x.ItemLore.Write(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Lore = Lore.FromId(worldData.Object, loreId)
                worldData.Verify(Sub(x) x.ItemLore.Write(id, loreId))
            End Sub)
    End Sub
    <Fact>
    Sub remove_lore_assignment()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Sub(x) x.ItemLore.ClearForItem(It.IsAny(Of Long)))
                subject.Lore = Nothing
                worldData.Verify(Sub(x) x.ItemLore.ClearForItem(id))
            End Sub)
    End Sub
    <Fact>
    Sub assign_a_name()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemName = "one"
                worldData.Setup(Sub(x) x.Item.WriteName(It.IsAny(Of Long), It.IsAny(Of String)))
                subject.Name = itemName
                worldData.Verify(Sub(x) x.Item.WriteName(id, itemName))
            End Sub)
    End Sub
    <Fact>
    Sub have_encumbrance()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Encumbrance.ShouldBe(0L)
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 29L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, 29L))
            End Sub)
    End Sub
End Class
