Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("SpaceSord",
                   5,
            MakeDictionary(
                (DungeonLevel.Moon, MakeHashSet(LocationType.Moon))),
            MakeDictionary(
                (DungeonLevel.Moon, "1d1")),
            MakeList(EquipSlot.Weapon),
            10,
            5)
    End Sub
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 100
        End Get
    End Property
End Class
