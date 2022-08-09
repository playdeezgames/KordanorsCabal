Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Shortsword",
                   5,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (OldDungeonLevel.Level1, "2d6"),
                (OldDungeonLevel.Level2, "1d6")),
            MakeList(EquipSlot.Weapon),
            5,
            MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 4
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 2
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 20
        End Get
    End Property
End Class
