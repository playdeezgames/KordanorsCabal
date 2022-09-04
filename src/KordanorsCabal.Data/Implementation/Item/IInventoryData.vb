Public Interface IInventoryData
    Sub ClearForCharacter(characterId As Long)
    Function ReadForLocation(locationId As Long) As Long?
    Function CreateForLocation(locationId As Long) As Long
    Function ReadForCharacter(characterId As Long) As Long?
    Function CreateForCharacter(characterId As Long) As Long
End Interface
