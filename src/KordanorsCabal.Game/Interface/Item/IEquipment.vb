Public Interface IEquipment
    Inherits IBaseThingie
    ReadOnly Property CanEquip As Boolean
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot)
    Function EquippedBuff(statisticType As IStatisticType) As Long?
End Interface
