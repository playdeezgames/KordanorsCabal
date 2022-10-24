Public Class Inventory
    Inherits BaseThingie
    Implements IInventory
    Sub New(worldData As IWorldData, inventoryId As Long)
        MyBase.New(worldData, inventoryId)
    End Sub
    Shared Function FromId(worldData As IWorldData, inventoryId As Long?) As IInventory
        Return If(inventoryId.HasValue, New Inventory(worldData, inventoryId.Value), Nothing)
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
        Dim oldInventoryId As Long? = WorldData.InventoryItem.ReadForItem(item.Id)
        If oldInventoryId.HasValue AndAlso oldInventoryId.Value = Id Then
            Return
        End If
        WorldData.InventoryItem.Write(Id, item.Id)
        Dim eventName = WorldData.ItemTypeEvent.Read(item.ItemType.Id, ItemType.AddToInventoryEventId)
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, Id, item.Id)
        End If
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
    Public ReadOnly Property Location As ILocation Implements IInventory.Location
        Get
            Return Game.Location.FromId(WorldData, WorldData.Inventory.ReadLocation(Id))
        End Get
    End Property

    Public ReadOnly Property Character As ICharacter Implements IInventory.Character
        Get
            Return Game.Character.FromId(WorldData, WorldData.Inventory.ReadCharacter(Id))
        End Get
    End Property
End Class
