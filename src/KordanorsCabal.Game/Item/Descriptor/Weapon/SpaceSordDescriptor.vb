Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.SpaceSord,
            5,
            MakeDictionary(
                (6L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 8L)))),
            MakeDictionary(
                (6L, "1d1")),
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            10,
            5,,
            100)
    End Sub
End Class
