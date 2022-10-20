Public Interface IInventory
    Inherits IBaseThingie
    ReadOnly Property ItemsOfType(itemType As IItemType) As IEnumerable(Of IItem)
    ReadOnly Property Items As IReadOnlyList(Of IItem)
    Sub Add(item As IItem)
    ReadOnly Property Location As ILocation
    ReadOnly Property TotalEncumbrance As Long
    ReadOnly Property IsEmpty As Boolean
End Interface
