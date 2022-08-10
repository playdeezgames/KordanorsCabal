Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SpaceSord,
            "SpaceSord",
                   5,
            MakeDictionary(
                (6L, MakeHashSet(LocationType.Moon))),
            MakeDictionary(
                (6L, "1d1")),
            MakeList(EquipSlot.FromName(Weapon)),,
            10,
            5,,
            100)
    End Sub
End Class
