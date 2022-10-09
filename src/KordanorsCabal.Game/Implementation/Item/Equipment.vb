Public Class Equipment
    Inherits BaseThingie
    Implements IEquipment

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IEquipment
        Return If(id.HasValue, New Equipment(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property CanEquip As Boolean Implements IEquipment.CanEquip
        Get
            Return Item.FromId(WorldData, Id).ItemType.EquipSlots.Any
        End Get
    End Property
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot) Implements IEquipment.EquipSlots
        Get
            Return Item.FromId(WorldData, Id).ItemType.EquipSlots
        End Get
    End Property
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long? Implements IEquipment.EquippedBuff
        Return Item.FromId(WorldData, Id).ItemType.EquippedBuff(statisticType)
    End Function
End Class
