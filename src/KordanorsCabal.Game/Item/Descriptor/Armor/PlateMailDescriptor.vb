Friend Class PlateMailDescriptor
    Inherits ItemTypeDescriptor
    Sub New()
        MyBase.New(
            "Platemail",
            40.0!,
            MakeDictionary(
                (OldDungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd)),
                (OldDungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (OldDungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
                MakeDictionary((OldDungeonLevel.Level2, "1d6")),
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
