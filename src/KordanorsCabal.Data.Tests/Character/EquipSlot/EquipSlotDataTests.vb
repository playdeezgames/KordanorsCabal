﻿Public Class EquipSlotDataTests
    Inherits WorldDataSubobjectTests(Of IEquipSlotData)
    Sub New()
        MyBase.New(Function(x) x.EquipSlot)
    End Sub
    <Fact>
    Sub ShouldQueryTheStoreForTheNameOfAnEquipSlot()
        WithSubobject(
            Sub(store, checker, subject)
                Dim equipSlot = 1L
                store.SetupGet(Function(x) x.Column).Returns((New Mock(Of IStoreColumn)).Object)
                subject.ReadName(equipSlot).ShouldBeNull
                store.Verify(
                    Function(x) x.Column.ReadString(Of Long)(
                    It.IsAny(Of Action),
                    Tables.EquipSlots,
                    Columns.EquipSlotNameColumn,
                    (Columns.EquipSlotIdColumn, equipSlot)))
            End Sub)
    End Sub
End Class
