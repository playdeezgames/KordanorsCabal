﻿Public Interface IRoute
    Inherits IBaseThingie
    Property RouteType As IRouteType
    Sub Destroy()
    Function CanMove(character As ICharacter) As Boolean
    ReadOnly Property ToLocation As ILocation
    Function Move(character As ICharacter) As ILocation
End Interface
