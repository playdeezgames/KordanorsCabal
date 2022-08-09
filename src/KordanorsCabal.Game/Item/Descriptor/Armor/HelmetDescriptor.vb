Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Helmet",
            2.0!,
            MakeDictionary(
                (OldDungeonLevel.Level1, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd))),
                MakeDictionary((OldDungeonLevel.Level1, "3d6")),
                MakeList(EquipSlot.Head),
                2,
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
