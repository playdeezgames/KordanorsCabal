﻿Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Trousers,
            "Trousers",,,,
            MakeList(EquipSlotDescriptor.FromName(Legs)),,,,,,,,,
            100,
            MakeList(ShoppeType.BlackMarket))
    End Sub

End Class
