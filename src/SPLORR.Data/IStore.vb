Public Interface IStore
    Inherits IStoreMeta, IStoreColumn, IStoreRecord, IStoreClear, IStoreReplace, IStoreCreate
    ReadOnly Property Count As IStoreCount
    ReadOnly Property Primitive As IStorePrimitive
End Interface
