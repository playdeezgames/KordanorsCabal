Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SpaceSord,
            "SpaceSord",
                   5,
            MakeDictionary(
                (OldDungeonLevel.Moon, MakeHashSet(LocationType.Moon))),
            MakeDictionary(
                (OldDungeonLevel.Moon, "1d1")),
            MakeList(EquipSlot.FromName(Weapon)),,
            10,
            5,,
            100)
    End Sub
End Class
