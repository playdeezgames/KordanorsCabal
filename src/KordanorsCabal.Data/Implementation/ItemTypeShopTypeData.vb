Friend Class ItemTypeShopTypeData
    Implements IItemTypeShopTypeData

    Public Function ReadForTransactionType(itemTypeId As Long, transationTypeId As Long) As IEnumerable(Of Long) Implements IItemTypeShopTypeData.ReadForTransactionType
        Return Array.Empty(Of Long) 'TODO: implement
    End Function
End Class
