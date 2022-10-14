Public Interface IStore
    Inherits IStoreRecord, IStoreReplace, IStoreCreate
    ReadOnly Property Count As IStoreCount
    ReadOnly Property Primitive As IStorePrimitive
    ReadOnly Property Clear As IStoreClear
    ReadOnly Property Meta As IStoreMeta
    ReadOnly Property Column As IStoreColumn
End Interface
