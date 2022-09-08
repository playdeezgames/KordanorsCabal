Friend Class RingOfHPDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RingOfHP,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 7L), EquipSlot.FromId(StaticWorldData.World, 8L)),
            New Dictionary(Of Long, Long) From {{6, 1}})
    End Sub
End Class
