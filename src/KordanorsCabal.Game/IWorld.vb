Public Interface IWorld
    ReadOnly Property IsValid As Boolean
    Sub Start()
    ReadOnly Property PlayerCharacter As IPlayerCharacter
End Interface
