Public Interface ICharacterData
    Sub Clear(characterId As Long)
    Function ReadLocation(characterId As Long) As Long?
    Sub WriteLocation(characterId As Long, locationId As Long)
    Function ReadForLocation(locationId As Long) As IEnumerable(Of Long)
    Function ReadCharacterType(characterId As Long) As Long?
    Function Create(characterType As Long, locationId As Long) As Long
End Interface
