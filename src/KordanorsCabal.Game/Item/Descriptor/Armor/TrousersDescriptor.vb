Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            ItemType.Trousers,
            "Trousers",,,,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 5L)),,,,,,,,,
            100,
            MakeList(ShoppeType.BlackMarket))
    End Sub

End Class
