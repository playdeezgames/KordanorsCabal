Public Interface IItem
    Inherits IBaseThingie
    ReadOnly Property Name As String
    ReadOnly Property ItemType As IItemType
    ReadOnly Property Weapon As IWeapon
    ReadOnly Property Durability As IDurability
    ReadOnly Property Repair As IRepair
    ReadOnly Property Armor As IArmor
    ReadOnly Property Equipment As IEquipment
    ReadOnly Property Usage As IUsage

    Sub Purify()
    Sub Destroy()
    Sub Decay()
End Interface
