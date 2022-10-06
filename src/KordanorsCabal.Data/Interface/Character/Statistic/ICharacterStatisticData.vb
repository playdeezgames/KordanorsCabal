Public Interface ICharacterStatisticData
    Sub ClearForCharacter(characterId As Long)
    Function Read(characterId As Long, statisticType As Long) As Long?
    Sub Write(characterId As Long, statisticType As Long, statisticValue As Long)
End Interface
