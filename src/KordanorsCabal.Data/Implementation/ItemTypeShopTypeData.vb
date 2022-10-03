Friend Class ItemTypeShopTypeData
    Implements IItemTypeShopTypeData

    Public Function ReadForTransactionType(transationTypeId As Long) As IEnumerable(Of Long) Implements IItemTypeShopTypeData.ReadForTransactionType
        Throw New NotImplementedException()
    End Function
End Class
