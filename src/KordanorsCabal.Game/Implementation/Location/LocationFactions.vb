Public Class LocationFactions
    Inherits BaseThingie
    Implements ILocationFactions
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ILocationFactions
        Return If(id.HasValue, New LocationFactions(worldData, id.Value), Nothing)
    End Function
End Class
