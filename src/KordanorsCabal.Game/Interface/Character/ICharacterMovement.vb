﻿Public Interface ICharacterMovement
    Inherits IBaseThingie
    Function CanMove(direction As IDirection) As Boolean
    Function CanMoveBackward() As Boolean
    Function CanMoveLeft() As Boolean
    Function CanMoveRight() As Boolean
    Function Move(direction As IDirection) As Boolean
    Function CanMoveForward() As Boolean
    Function HasVisited(location As ILocation) As Boolean
    ReadOnly Property CanMap() As Boolean
    Property Direction As IDirection
    Property Location As ILocation
End Interface
