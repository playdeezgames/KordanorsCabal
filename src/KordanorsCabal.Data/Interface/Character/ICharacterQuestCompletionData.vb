Public Interface ICharacterQuestCompletionData
    Sub ClearForCharacter(characterId As Long)
    Function Read(characterId As Long, questId As Long) As Long?
    Sub Write(characterId As Long, questId As Long, count As Long)
End Interface
