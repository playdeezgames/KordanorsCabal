﻿Friend Class ShieldDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Shield,
            5,
            MakeDictionary(
                (1L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (2L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (3L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (4L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L))),
                (5L, MakeHashSet(LocationType.FromId(StaticWorldData.World, 5L), LocationType.FromId(StaticWorldData.World, 4L)))),
                MakeDictionary((1L, "3d6")),
                MakeList(EquipSlot.FromId(StaticWorldData.World, 2L)),,,,
                2,
                10,,
                3,
                MakeList(ShoppeType.Blacksmith),
                15,
                MakeList(ShoppeType.Blacksmith),
                6,
                MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
