﻿Public Class Inventory
    ReadOnly Property Id As Long
    Sub New(inventoryId As Long)
        Id = inventoryId
    End Sub
    Shared Function FromId(inventoryId As Long) As Inventory
        Return New Inventory(inventoryId)
    End Function
    ReadOnly Property IsEmpty As Boolean
        Get
            Return Not Items.Any
        End Get
    End Property

    ReadOnly Property Items As IReadOnlyList(Of Item)
        Get
            Return StaticWorldData.InventoryItem.ReadItems(Id).Select(AddressOf Item.FromId).ToList
        End Get
    End Property

    Public Sub Add(item As Item)
        StaticWorldData.InventoryItem.Write(Id, item.Id)
    End Sub
    ReadOnly Property ItemsOfType(itemType As ItemType) As IEnumerable(Of Item)
        Get
            Return Items.Where(Function(x) x.ItemType = itemType)
        End Get
    End Property

    ReadOnly Property TotalEncumbrance As Single
        Get
            Return Items.Sum(Function(x) x.Encumbrance)
        End Get
    End Property
End Class
