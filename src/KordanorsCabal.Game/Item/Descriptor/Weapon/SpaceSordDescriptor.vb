Friend Class SpaceSordDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.SpaceSord,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            10,
            5,,
            100)
    End Sub
End Class
