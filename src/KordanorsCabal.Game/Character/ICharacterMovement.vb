Public Interface ICharacterMovement
    Inherits IBaseThingie
    Function CanMove(direction As IDirection) As Boolean
    Function CanMoveBackward() As Boolean
    Function CanMoveLeft() As Boolean
    Function CanMoveRight() As Boolean
    ReadOnly Property Character As ICharacter
End Interface
