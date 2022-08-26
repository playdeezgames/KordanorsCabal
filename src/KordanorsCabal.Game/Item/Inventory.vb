Public Class Inventory
    Inherits BaseThingie
    Sub New(worldData As WorldData, inventoryId As Long)
        MyBase.New(worldData, inventoryId)
    End Sub
    Shared Function FromId(worldData As WorldData, inventoryId As Long) As Inventory
        Return New Inventory(worldData, inventoryId)
    End Function
    ReadOnly Property IsEmpty As Boolean
        Get
            Return Not Items.Any
        End Get
    End Property

    ReadOnly Property Items As IReadOnlyList(Of Item)
        Get
            Return WorldData.InventoryItem.ReadItems(Id).Select(Function(x) Item.FromId(WorldData, x)).ToList
        End Get
    End Property

    Public Sub Add(item As Item)
        WorldData.InventoryItem.Write(Id, item.Id)
    End Sub
    ReadOnly Property ItemsOfType(itemType As ItemType) As IEnumerable(Of Item)
        Get
            Return Items.Where(Function(x) x.ItemType = itemType)
        End Get
    End Property

    ReadOnly Property TotalEncumbrance As Long
        Get
            Return Items.Sum(Function(x) x.Encumbrance)
        End Get
    End Property
End Class
