Public Interface ICharacterItems
    Inherits IBaseThingie
    ReadOnly Property Inventory As IInventory
    ReadOnly Property CanBeBribedWith(itemType As IItemType) As Boolean
    Function HasItemType(itemType As IItemType) As Boolean
    Sub UseItem(item As IItem)
    Sub PurifyItems()
End Interface
