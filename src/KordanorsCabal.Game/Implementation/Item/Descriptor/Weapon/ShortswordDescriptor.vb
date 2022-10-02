Friend Class ShortswordDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Shortsword,
            MakeList(ShoppeType.Blacksmith),
            MakeList(ShoppeType.Blacksmith),
            MakeList(ShoppeType.Blacksmith))
    End Sub
End Class
