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
            Return True
        End Get
    End Property
End Class
