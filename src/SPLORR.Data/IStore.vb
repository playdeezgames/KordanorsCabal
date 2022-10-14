Public Interface IStore
    Inherits IStoreColumn, IStoreRecord, IStoreReplace, IStoreCreate
    ReadOnly Property Count As IStoreCount
    ReadOnly Property Primitive As IStorePrimitive
    ReadOnly Property Clear As IStoreClear
    ReadOnly Property Meta As IStoreMeta
End Interface
