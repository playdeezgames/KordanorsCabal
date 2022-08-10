﻿Friend Class ShortswordDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Shortsword,
            "Shortsword",
                   5,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.Dungeon)),
                (2L, MakeHashSet(LocationType.Dungeon)),
                (3L, MakeHashSet(LocationType.Dungeon)),
                (4L, MakeHashSet(LocationType.Dungeon)),
                (5L, MakeHashSet(LocationType.Dungeon))),
            MakeDictionary(
                (1L, "2d6"),
                (2L, "1d6")),
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
