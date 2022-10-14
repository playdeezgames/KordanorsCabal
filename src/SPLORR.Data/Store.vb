Public Class Store
    Inherits StoreBase
    Implements IStore
    Private templateFilename As String
    Public Sub New(templateFilename As String)
        MyBase.New(New Backer)
        Me.templateFilename = templateFilename
    End Sub
    Public ReadOnly Property Count As IStoreCount Implements IStore.Count
        Get
            Return New StoreCount(backer)
        End Get
    End Property

    Public ReadOnly Property Primitive As IStorePrimitive Implements IStore.Primitive
        Get
            Return New StorePrimitive(backer)
        End Get
    End Property

    Public ReadOnly Property Clear As IStoreClear Implements IStore.Clear
        Get
            Return New StoreClear(backer)
        End Get
    End Property

    Public ReadOnly Property Meta As IStoreMeta Implements IStore.Meta
        Get
            Return New StoreMeta(backer, templateFilename)
        End Get
    End Property

    Public ReadOnly Property Column As IStoreColumn Implements IStore.Column
        Get
            Return New StoreColumn(backer)
        End Get
    End Property

    Public ReadOnly Property Create As IStoreCreate Implements IStore.Create
        Get
            Return New StoreCreate(backer)
        End Get
    End Property

    Public ReadOnly Property Replace As IStoreReplace Implements IStore.Replace
        Get
            Return New StoreReplace(backer)
        End Get
    End Property

    Public ReadOnly Property Record As IStoreRecord Implements IStore.Record
        Get
            Return New StoreRecord(backer)
        End Get
    End Property
End Class
