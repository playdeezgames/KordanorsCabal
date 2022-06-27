Public Class Item
    ReadOnly Property Id As Long
    Sub New(itemId As Long)
        Id = itemId
    End Sub
    Shared Function FromId(itemId As Long) As Item
        Return New Item(itemId)
    End Function
    Shared Function Create(itemType As ItemType) As Item
        Return FromId(ItemData.Create(itemType))
    End Function

    Public ReadOnly Property ItemType As ItemType
        Get
            Return CType(ItemData.ReadItemType(Id).Value, ItemType)
        End Get
    End Property

    Public ReadOnly Property Name As String
        Get
            Return ItemType.Name
        End Get
    End Property

    Friend Sub Destroy()
        ItemData.Clear(Id)
    End Sub
End Class
