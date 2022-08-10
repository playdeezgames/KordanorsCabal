﻿Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shortsword,
            "Shortsword",
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
            MakeList(EquipSlot.FromName(Weapon)),,
            4,
            2,,
            20,,
            5,
            MakeList(ShoppeType.Blacksmith),
            25,
            MakeList(ShoppeType.Blacksmith),
            10,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
