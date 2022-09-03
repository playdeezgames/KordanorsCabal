Public Interface ICharacterLocationData
    Sub ClearForCharacter(characterId As Long)
    Sub Write(characterId As Long, locationId As Long)
    Function Read(characterId As Long, locationId As Long) As Boolean
End Interface
