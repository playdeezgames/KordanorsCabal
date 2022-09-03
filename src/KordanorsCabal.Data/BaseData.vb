Public MustInherit Class BaseData
    Protected ReadOnly Store As IStore
    Protected ReadOnly World As WorldData
    Sub New(store As IStore, world As WorldData)
        Me.Store = store
        Me.World = world
    End Sub
End Class
