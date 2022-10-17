Public Class EquipSlotData
    Inherits BaseData
    Implements IEquipSlotData
    Friend Const EquipSlotIdColumn = "EquipSlotId"
    Friend Const EquipSlotNameColumn = "EquipSlotName"

    Public Function ReadName(equipSlotId As Long) As String Implements IEquipSlotData.ReadName
        Return Store.Column.ReadString(
            AddressOf NoInitializer,
            EquipSlots,
            EquipSlotNameColumn,
            (EquipSlotIdColumn, equipSlotId))
    End Function
    Public Sub New(store As IStore, world As IWorldData)
        MyBase.New(store, world)
    End Sub
End Class
