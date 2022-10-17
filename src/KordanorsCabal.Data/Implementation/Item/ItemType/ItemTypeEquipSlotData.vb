Public Class ItemTypeEquipSlotData
    Inherits BaseData
    Implements IItemTypeEquipSlotData
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
    Public Function ReadForItemType(itemTypeId As Long) As IEnumerable(Of Long) Implements IItemTypeEquipSlotData.ReadForItemType
        Return Store.Record.WithValues(Of Long, Long)(
            AddressOf NoInitializer,
            ItemTypeEquipSlots,
            EquipSlotIdColumn,
            (ItemTypeIdColumn, itemTypeId))
    End Function
End Class
