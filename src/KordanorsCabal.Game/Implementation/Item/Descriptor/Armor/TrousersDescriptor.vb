Friend Class TrousersDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Trousers,
            MakeList(EquipSlot.FromId(StaticWorldData.World, 5L)),,,,,,,,
            100,
            MakeList(ShoppeType.BlackMarket))
    End Sub

End Class
