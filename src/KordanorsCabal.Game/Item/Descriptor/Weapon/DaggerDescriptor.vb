Friend Class DaggerDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New("Dagger",
                   1,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (OldDungeonLevel.Level1, "4d6"),
                (OldDungeonLevel.Level2, "2d6")),
            MakeList(EquipSlot.Weapon),
            1,
            MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property AttackDice As Long
        Get
            Return 2
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDamage As Long?
        Get
            Return 1
        End Get
    End Property
    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 10
        End Get
    End Property
End Class
