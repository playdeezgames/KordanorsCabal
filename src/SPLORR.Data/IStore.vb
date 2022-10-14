Public Interface IStore
    Inherits IStoreMeta, IStorePrimitive, IStoreColumn, IStoreRecord, IStoreClear, IStoreReplace, IStoreCreate
    ReadOnly Property Count As IStoreCount
End Interface
