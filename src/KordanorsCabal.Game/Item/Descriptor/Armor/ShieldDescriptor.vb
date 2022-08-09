Friend Class ShieldDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Shield",
            5.0!,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
                MakeDictionary((OldDungeonLevel.Level1, "3d6")),
                MakeList(EquipSlot.Shield),
                3,
                MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 10
        End Get
    End Property

    Public Overrides ReadOnly Property DefendDice As Long
        Get
            Return 2
        End Get
    End Property
End Class
