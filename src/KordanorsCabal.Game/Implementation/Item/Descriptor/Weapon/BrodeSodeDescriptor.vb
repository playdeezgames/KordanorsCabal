﻿Friend Class BrodeSodeDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.BrodeSode,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 1L)),,
            3,,
            40,
            20,
            MakeList(ShoppeType.Blacksmith),
            100,
            MakeList(ShoppeType.Blacksmith),
            40,
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
