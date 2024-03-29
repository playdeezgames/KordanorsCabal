﻿Public Interface IItem
    Inherits IBaseThingie
    Property Name As String
    Property ItemType As IItemType
    ReadOnly Property Weapon As IWeapon
    ReadOnly Property Durability As IDurability
    ReadOnly Property Repair As IRepair
    ReadOnly Property Armor As IArmor
    ReadOnly Property Equipment As IEquipment
    ReadOnly Property Events As IItemEvents
    Sub Destroy()
    ReadOnly Property Encumbrance As Long
    Property Lore As ILore
    ReadOnly Property Inventory As IInventory
End Interface
