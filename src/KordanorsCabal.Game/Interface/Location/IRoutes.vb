Public Interface IRoutes
    Inherits IBaseThingie
    Function Exists(direction As IDirection) As Boolean
    Function Find(direction As IDirection) As IRoute
End Interface
