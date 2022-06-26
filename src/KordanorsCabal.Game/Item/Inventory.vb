Public Class Inventory
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
            Return InventoryItemData.ReadItems(Id).Select(AddressOf Item.FromId).ToList
        End Get
    End Property

    Public Sub Add(item As Item)
        InventoryItemData.Write(Id, item.Id)
    End Sub
End Class
