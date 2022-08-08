Friend Class BrodeSodeDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "BrodeSode",
            10,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (DungeonLevel.Level2, "2d6"),
                (DungeonLevel.Level3, "1d6")),
                20,
                MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Weapon}
        End Get
    End Property

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
