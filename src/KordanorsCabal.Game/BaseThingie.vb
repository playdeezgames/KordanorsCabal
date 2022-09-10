Public Class BaseThingie
    Implements IBaseThingie
    Protected ReadOnly WorldData As IWorldData
    Public ReadOnly Property Id As Long Implements IBaseThingie.Id
    Sub New(worldData As IWorldData, id As Long)
        Me.WorldData = worldData
        Me.Id = id
    End Sub
End Class
