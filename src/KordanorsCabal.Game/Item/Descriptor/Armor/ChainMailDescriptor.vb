Friend Class ChainMailDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Chainmail",
            20.0!,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.DungeonDeadEnd, LocationType.Dungeon))),
            MakeDictionary((DungeonLevel.Level1, "1d6")),
            10,
            MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Torso}
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumDurability As Long?
        Get
            Return 25
        End Get
    End Property

    Public Overrides ReadOnly Property DefendDice As Long
        Get
            Return 2
        End Get
    End Property
End Class
