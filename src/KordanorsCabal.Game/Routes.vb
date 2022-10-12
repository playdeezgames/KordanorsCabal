Public Class Routes
    Inherits BaseThingie
    Implements IRoutes
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IRoutes
        Return If(id.HasValue, New Routes(worldData, id.Value), Nothing)
    End Function
End Class
