Public Interface ICharacterRepair
    Inherits IBaseThingie
    Function HasItemsToRepair(shoppeType As IShoppeType) As Boolean
    ReadOnly Property ItemsToRepair(shoppeType As IShoppeType) As IEnumerable(Of IItem)
End Interface
