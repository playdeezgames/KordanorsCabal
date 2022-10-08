Public Class Inventory
    Inherits BaseThingie
    Implements IInventory
    Sub New(worldData As IWorldData, inventoryId As Long)
        MyBase.New(worldData, inventoryId)
    End Sub
    Shared Function FromId(worldData As IWorldData, inventoryId As Long) As IInventory
        Return New Inventory(worldData, inventoryId)
    End Function
    ReadOnly Property IsEmpty As Boolean Implements IInventory.IsEmpty
        Get
            Return Not Items.Any
        End Get
    End Property

    ReadOnly Property Items As IReadOnlyList(Of IItem) Implements IInventory.Items
        Get
            Return WorldData.InventoryItem.ReadItems(Id).Select(Function(x) Item.FromId(WorldData, x)).ToList
        End Get
    End Property

    Public Sub Add(item As IItem) Implements IInventory.Add
        WorldData.InventoryItem.Write(Id, item.Id)
    End Sub
    ReadOnly Property ItemsOfType(itemType As IItemType) As IEnumerable(Of IItem) Implements IInventory.ItemsOfType
        Get
            Return Items.Where(Function(x) x.ItemType.Id = itemType.Id)
        End Get
    End Property

    ReadOnly Property TotalEncumbrance As Long Implements IInventory.TotalEncumbrance
        Get
            Return Items.Sum(Function(x) x.ItemType.Encumbrance)
        End Get
    End Property
End Class
