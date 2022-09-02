Friend Class TrousersDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Trousers,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 5L)),,,,,,,,
            100,
            MakeList(ShoppeType.BlackMarket))
    End Sub

End Class
