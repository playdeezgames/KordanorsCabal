﻿Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shortsword,
            "Shortsword",
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
            MakeList(EquipSlot.Weapon),,
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
