Public Class ItemTypeEquipSlotDataTests
    Inherits WorldDataSubobjectTests(Of IItemTypeEquipSlotData)
    Sub New()
        MyBase.New(Function(x) x.ItemTypeEquipSlot)
    End Sub
    <Fact>
    Sub item_type_equip_slot_can_be_read_from_the_data_store()
        WithSubobject(
            Sub(store, checker, subject)
                Const itemTypeId = 1L
                store.SetupGet(Function(x) x.Record).Returns((New Mock(Of IStoreRecord)).Object)
                subject.ReadForItemType(itemTypeId)
                store.Verify(
                    Function(x) x.Record.WithValues(Of Long, Long)(
                        It.IsAny(Of Action),
                        Tables.ItemTypeEquipSlots,
                        Columns.EquipSlotIdColumn,
                        (Columns.ItemTypeIdColumn, itemTypeId)))
            End Sub)
    End Sub
End Class
