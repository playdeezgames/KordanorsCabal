Friend Class SpaceSordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("SpaceSord",
                   5,
            MakeDictionary(
                (DungeonLevel.Moon, MakeHashSet(LocationType.Moon))),
            MakeDictionary(
                (DungeonLevel.Moon, "1d1")),
            MakeList(EquipSlot.Weapon))
    End Sub

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 10
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 5
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 100
        End Get
    End Property
End Class
