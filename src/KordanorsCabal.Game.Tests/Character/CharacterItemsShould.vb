﻿Public Class CharacterItemsShould
    Inherits ThingieShould(Of ICharacterItems)
    Sub New()
        MyBase.New(Function(w, i) CharacterItems.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub have_can_be_bribed_with()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 2L
                Const itemTypeId = 14L

                Dim characterData = New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(X) X.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)

                subject.CanBeBribedWith(Game.ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeFalse

                characterData.Verify(Function(x) x.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId))
                characterData.VerifyNoOtherCalls()
            End Sub)
    End Sub
    <Fact>
    Sub have_inventory()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                subject.Inventory.Id.ShouldBe(0)
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_item_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                subject.HasItemType(ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
            End Sub)
    End Sub
    <Fact>
    Sub have_use_item()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemId = 2L
                Const itemTypeId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.UseItem(Item.FromId(worldData.Object, itemId))
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 2L))
            End Sub)
    End Sub
    <Fact>
    Sub purify_items()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.PurifyItems()
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
End Class