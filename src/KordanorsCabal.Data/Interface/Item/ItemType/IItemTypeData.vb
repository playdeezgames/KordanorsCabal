Public Interface IItemTypeData
    Function ReadName(itemTypeId As Long) As String
    Function ReadIsConsumed(itemTypeId As Long) As Long?
End Interface
