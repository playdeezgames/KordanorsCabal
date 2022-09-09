﻿Public Interface IPlayerData
    Sub ClearForCharacter(characterId As Long)
    Function Read() As Long?
    Function ReadDirection() As Long?
    Function ReadMode() As Long?
    Sub Write(characterId As Long, direction As Long, mode As Long)
    Sub WriteDirection(directionId As Long)
    Sub WriteMode(value As Long)
End Interface
