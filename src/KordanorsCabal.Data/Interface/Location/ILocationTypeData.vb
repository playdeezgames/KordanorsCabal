Public Interface ILocationTypeData
    Function ReadName(locationTypeId As Long) As String
    Function ReadIsDungeon(locationTypeId As Long) As Boolean
    Function ReadCanMap(locationTypeId As Long) As Boolean
    Function ReadRequiresMP(locationTypeId As Long) As Boolean
End Interface
