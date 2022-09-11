Public Interface IInventory
    Inherits IBaseThingie
    ReadOnly Property ItemsOfType(itemType As OldItemType) As IEnumerable(Of Item)
    ReadOnly Property Items As IReadOnlyList(Of Item)
    Sub Add(item As Item)
    ReadOnly Property TotalEncumbrance As Long
    ReadOnly Property IsEmpty As Boolean
End Interface
