Public Class Usage
    Inherits BaseThingie
    Implements IUsage

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Friend Shared Function FromId(worldData As IWorldData, id As Long?) As IUsage
        Return If(id.HasValue, New Usage(worldData, id.Value), Nothing)
    End Function
End Class
