Public Class ItemShould
    Inherits ThingieShould(Of IItem)
    Sub New()
        MyBase.New(AddressOf Item.FromId)
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
                subject.Decay()
            End Sub)
    End Sub
End Class
