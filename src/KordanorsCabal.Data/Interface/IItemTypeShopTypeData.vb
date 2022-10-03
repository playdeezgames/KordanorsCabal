Public Interface IItemTypeShopTypeData
    Function ReadForTransactionType(itemTypeId As Long, transationTypeId As Long) As IEnumerable(Of Long)
End Interface
