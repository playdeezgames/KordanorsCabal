﻿Public Interface IStore
    ReadOnly Property Record As IStoreRecord
    ReadOnly Property Count As IStoreCount
    ReadOnly Property Primitive As IStorePrimitive
    ReadOnly Property Clear As IStoreClear
    ReadOnly Property Meta As IStoreMeta
    ReadOnly Property Column As IStoreColumn
    ReadOnly Property Create As IStoreCreate
    ReadOnly Property Replace As IStoreReplace
End Interface
