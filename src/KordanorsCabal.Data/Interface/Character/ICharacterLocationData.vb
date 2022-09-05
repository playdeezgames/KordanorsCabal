Public Interface ICharacterLocationData
    Sub ClearForCharacter(characterId As Long)
    Function Read(characterId As Long, locationId As Long) As Boolean
    Sub Write(characterId As Long, locationId As Long)
End Interface
