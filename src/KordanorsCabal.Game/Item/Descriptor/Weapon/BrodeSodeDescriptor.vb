Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "BrodeSode",
            10,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (OldDungeonLevel.Level2, "2d6"),
                (OldDungeonLevel.Level3, "1d6")),
                MakeList(EquipSlot.Weapon),
                20,
                MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 6
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 3
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 40
        End Get
    End Property
End Class
