Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Trousers,
            "Trousers",,,,
            MakeList(EquipSlot.Legs.ToDescriptor),,,,,,,,,
            100,
            MakeList(ShoppeType.BlackMarket))
    End Sub

End Class
