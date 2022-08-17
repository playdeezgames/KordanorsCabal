Public Class BaseThingie
    Protected ReadOnly WorldData As WorldData
    Public ReadOnly Id As Long
    Sub New(worldData As WorldData, id As Long)
        Me.WorldData = worldData
        Me.Id = id
    End Sub
End Class
