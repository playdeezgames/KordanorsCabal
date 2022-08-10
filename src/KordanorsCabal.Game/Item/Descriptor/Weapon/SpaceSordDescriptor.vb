Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.SpaceSord,
            "SpaceSord",
                   5,
            MakeDictionary(
                (DungeonLevel.Moon, MakeHashSet(LocationType.Moon))),
            MakeDictionary(
                (DungeonLevel.Moon, "1d1")),
            MakeList(EquipSlot.Weapon.ToDescriptor),,
            10,
            5,,
            100)
    End Sub
End Class
