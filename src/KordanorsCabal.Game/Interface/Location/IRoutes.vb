Public Interface IRoutes
    Inherits IBaseThingie
    Function HasRoute(direction As IDirection) As Boolean
    Function GetRoute(direction As IDirection) As IRoute
End Interface
