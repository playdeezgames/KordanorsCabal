Public Interface ICharacterQuestData
    Sub Clear(characterId As Long, quest As Long)
    Sub ClearForCharacter(characterId As Long)
    Function Read(characterId As Long, quest As Long) As Boolean
    Sub Write(characterId As Long, quest As Long)
End Interface
