Public Interface ICharacterQuestData
    Sub Write(characterId As Long, quest As Long)
    Function Exists(characterId As Long, quest As Long) As Boolean
    Sub ClearForCharacter(characterId As Long)
    Sub Clear(characterId As Long, quest As Long)
End Interface
