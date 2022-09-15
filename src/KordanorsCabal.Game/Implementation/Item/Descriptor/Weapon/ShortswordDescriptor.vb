﻿Friend Class ShortswordDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Shortsword,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            4,
            2,,
            20,
            5,
            MakeList(ShoppeType.Blacksmith),
            25,
            MakeList(ShoppeType.Blacksmith),
            10,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class