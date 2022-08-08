﻿Friend Class HelmetDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            "Helmet",
            2.0!,
            MakeDictionary(
                (DungeonLevel.Level1, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level2, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level3, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level4, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd)),
                (DungeonLevel.Level5, MakeHashSet(LocationType.Dungeon, LocationType.DungeonDeadEnd))),
                MakeDictionary((DungeonLevel.Level1, "3d6")),
                2,
                MakeList(ShoppeType.Blacksmith))
    End Sub

    Public Overrides ReadOnly Property EquipSlots As IEnumerable(Of EquipSlot)
        Get
            Return New List(Of EquipSlot) From {Game.EquipSlot.Head}
        End Get
    End Property

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
