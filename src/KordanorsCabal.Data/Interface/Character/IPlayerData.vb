Public Interface IPlayerData
    Sub ClearForCharacter(characterId As Long)
    Function Read() As Long?
    Function ReadDirection() As Long?
    Function ReadPlayerMode() As Long?
    Sub Write(characterId As Long, direction As Long, mode As Long, shoppeType As Long?)
    Sub WriteDirection(directionId As Long)
    Sub WritePlayerMode(value As Long)
    Function ReadShoppeType() As Long?
    Sub WriteShoppeType(shoppeTypeId As Long?)
End Interface
