Public Class CharacterEquipment
    Inherits SubcharacterBase
    Implements ICharacterEquipment

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterEquipment
        Return If(character IsNot Nothing, New CharacterEquipment(worldData, character), Nothing)
    End Function
    Private Function WearOneWeapon() As IItemType
        WearOneWeapon = Nothing
        Dim items = EquippedItems.Where(Function(x) x.Durability.Maximum IsNot Nothing AndAlso x.Weapon.IsWeapon).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.Durability.Reduce(1)
            If item.Durability.IsBroken Then
                WearOneWeapon = item.ItemType
                item.Destroy()
            End If
        End If
    End Function
    Function DoWeaponWear(wear As Long) As IEnumerable(Of IItemType) Implements ICharacterEquipment.DoWeaponWear
        Dim result As New List(Of IItemType)
        While wear > 0
            Dim brokenItemType = WearOneWeapon()
            If brokenItemType IsNot Nothing Then
                result.Add(brokenItemType)
            End If
            wear -= 1
        End While
        Return result
    End Function
    Function DoArmorWear(wear As Long) As IEnumerable(Of IItemType) Implements ICharacterEquipment.DoArmorWear
        Dim result As New List(Of IItemType)
        While wear > 0
            Dim brokenItemType = WearOneArmor()
            If brokenItemType.HasValue Then
                result.Add(ItemType.FromId(WorldData, brokenItemType.Value))
            End If
            wear -= 1
        End While
        Return result
    End Function
    Function CurrentEquipment(equipSlot As IEquipSlot) As IItem Implements ICharacterEquipment.CurrentEquipment
        Return Item.FromId(WorldData, WorldData.CharacterEquipSlot.ReadForCharacterEquipSlot(Id, equipSlot.Id))
    End Function
    ReadOnly Property EquippedSlots As IEnumerable(Of IEquipSlot) Implements ICharacterEquipment.EquippedSlots
        Get
            Return WorldData.CharacterEquipSlot.ReadEquipSlotsForCharacter(Id).Select(Function(x) New EquipSlot(WorldData, x))
        End Get
    End Property
    Public Sub Unequip(equipSlot As IEquipSlot) Implements ICharacterEquipment.Unequip
        Dim item = CurrentEquipment(equipSlot)
        If item IsNot Nothing Then
            character.Items.Inventory.Add(item)
            WorldData.CharacterEquipSlot.Clear(Id, equipSlot.Id)
        End If
    End Sub
    Public Sub Equip(item As IItem) Implements ICharacterEquipment.Equip
        If item.Equipment.CanEquip Then
            WorldData.InventoryItem.ClearForItem(item.Id)
            Dim equipSlots = item.Equipment.EquipSlots
            Dim availableEquipSlots = equipSlots.Where(Function(x) CurrentEquipment(x) Is Nothing)
            Dim equipSlot = If(availableEquipSlots.Any, availableEquipSlots.First, equipSlots.First)
            Dim oldItem = CurrentEquipment(equipSlot)
            If oldItem IsNot Nothing Then
                character.Items.Inventory.Add(oldItem)
            End If
            WorldData.CharacterEquipSlot.Write(Id, equipSlot.Id, item.Id)
            character.EnqueueMessage(Nothing, $"You equip {item.Name} to {equipSlot.Name}.")
            Return
        End If
        character.EnqueueMessage(Nothing, $"You cannot equip {item.Name}!")
    End Sub
    Public ReadOnly Property HasEquipment As Boolean Implements ICharacterEquipment.HasEquipment
        Get
            Return EquippedItems.Any
        End Get
    End Property
    Private Function WearOneArmor() As Long?
        Dim items = EquippedItems.Where(Function(x) x.Durability.Maximum IsNot Nothing AndAlso x.Armor.IsArmor).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.Durability.Reduce(1)
            If item.Durability.IsBroken Then
                WearOneArmor = If(item.ItemType IsNot Nothing, item.ItemType.Id, Nothing)
                item.Destroy()
            End If
        End If
    End Function
    ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
End Class
