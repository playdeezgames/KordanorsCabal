﻿Public Interface ICharacterType
    Inherits IBaseThingie
    ReadOnly Property IsUndead As Boolean
    ReadOnly Property Name As String
    ReadOnly Property Spawning As ICharacterTypeSpawning
    ReadOnly Property Combat As ICharacterTypeCombat
End Interface
