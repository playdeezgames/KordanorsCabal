Public Interface IStore
    Inherits IStoreMeta, IStoreColumn, IStoreRecord, IStoreReplace, IStoreCreate
    ReadOnly Property Count As IStoreCount
    ReadOnly Property Primitive As IStorePrimitive
    ReadOnly Property Clear As IStoreClear
End Interface
