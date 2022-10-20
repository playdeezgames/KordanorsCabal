Public Class ItemTypeEquip
    Inherits BaseThingie
    Implements IItemTypeEquip
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IItemTypeEquip
        Return If(id.HasValue, New ItemTypeEquip(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot) Implements IItemTypeEquip.EquipSlots
        Get
            Return WorldData.ItemTypeEquipSlot.ReadForItemType(Id).Select(Function(x) EquipSlot.FromId(WorldData, x))
        End Get
    End Property
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long? Implements IItemTypeEquip.EquippedBuff
        Return WorldData.ItemTypeCharacterStatisticBuff.Read(Id, statisticType.Id)
    End Function
End Class
