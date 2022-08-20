Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SpaceSord,
            "SpaceSord",
                   5,
            MakeDictionary(
                (6L, MakeHashSet(LocationType.FromId(8L)))),
            MakeDictionary(
                (6L, "1d1")),
            MakeList(EquipSlot.FromId(1L)),,
            10,
            5,,
            100)
    End Sub
End Class
