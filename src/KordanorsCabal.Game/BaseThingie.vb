Public Class BaseThingie
    Protected ReadOnly WorldData As IWorldData
    Public ReadOnly Id As Long
    Sub New(worldData As IWorldData, id As Long)
        Me.WorldData = worldData
        Me.Id = id
    End Sub
End Class
