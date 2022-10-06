Public MustInherit Class BaseData
    Protected ReadOnly Store As IStore
    Protected ReadOnly World As IWorldData
    Sub New(store As IStore, world As IWorldData)
        Me.Store = store
        Me.World = world
    End Sub
End Class
