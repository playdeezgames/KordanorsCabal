Public MustInherit Class BaseData
    Protected ReadOnly Store As SPLORR.Data.Store
    Sub New(store As SPLORR.Data.Store)
        Me.Store = store
    End Sub
End Class
