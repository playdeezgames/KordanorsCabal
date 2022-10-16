Public Interface ICharacterInteraction
    Inherits IBaseThingie
    Sub Interact()
    ReadOnly Property CanInteract As Boolean
    Sub Gamble()
    ReadOnly Property CanGamble As Boolean
End Interface
