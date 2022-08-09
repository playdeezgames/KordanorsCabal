Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Shortsword",
                   5,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (DungeonLevel.Level1, "2d6"),
                (DungeonLevel.Level2, "1d6")),
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
