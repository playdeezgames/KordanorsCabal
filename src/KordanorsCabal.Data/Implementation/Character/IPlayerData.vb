Public Interface IPlayerData
    Sub ClearForCharacter(characterId As Long)
    Function ReadMode() As Long?
    Sub WriteMode(value As Long)
    Function ReadDirection() As Long?
    Sub WriteDirection(directionId As Long)
    Sub Write(characterId As Long, direction As Long, mode As Long)
    Function Read() As Long?
End Interface
