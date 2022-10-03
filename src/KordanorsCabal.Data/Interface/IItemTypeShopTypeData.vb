Public Interface IItemTypeShopTypeData
    Function ReadForTransactionType(transationTypeId As Long) As IEnumerable(Of Long)
End Interface
