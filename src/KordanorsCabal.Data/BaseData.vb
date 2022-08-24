Public MustInherit Class BaseData
    Protected ReadOnly Store As SPLORR.Data.Store
    Protected ReadOnly World As WorldData
    Sub New(store As Store, world As WorldData)
        Me.Store = store
        Me.World = world
    End Sub
End Class
