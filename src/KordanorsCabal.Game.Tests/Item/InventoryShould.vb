Public Class InventoryShould
    Inherits ThingieShould(Of IInventory)

    Public Sub New()
        MyBase.New(Function(w, i) Inventory.FromId(w, i))
    End Sub
    <Fact>
    Sub have_location()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadLocation(It.IsAny(Of Long)))
                subject.Location.ShouldBeNull
                worldData.Verify(Function(x) x.Inventory.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_items_of_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                subject.ItemsOfType(ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeEmpty
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_items()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                subject.Items.ShouldBeEmpty
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(id))
            End Sub)
    End Sub
    <Fact>
    Sub add_item()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemId = 2L
                Const itemTypeId = 3L
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Sub(x) x.InventoryItem.Write(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                subject.Add(Item.FromId(worldData.Object, itemId))
                worldData.Verify(Sub(x) x.InventoryItem.Write(id, itemId))
                worldData.Verify(Sub(x) x.InventoryItem.ReadForItem(itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, ItemType.AddToInventoryEventId))
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
            End Sub)
    End Sub
    <Fact>
    Sub have_total_encumbrance()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                subject.TotalEncumbrance.ShouldBe(0)
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_empty()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                subject.IsEmpty.ShouldBeTrue
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_character()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadCharacter(It.IsAny(Of Long)))
                subject.Character.ShouldBeNull
                worldData.Verify(Function(x) x.Inventory.ReadCharacter(id))
            End Sub)
    End Sub
End Class
