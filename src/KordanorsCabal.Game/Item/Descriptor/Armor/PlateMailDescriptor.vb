Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            "Platemail",
            40.0!,
            MakeDictionary(
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
                MakeDictionary((DungeonLevel.Level2, "1d6")),
                MakeList(EquipSlot.Torso),
                50,
                MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 50
        End Get
    End Property

    Public Overrides ReadOnly Property DefendDice As Long
        Get
            Return 4
        End Get
    End Property
End Class
