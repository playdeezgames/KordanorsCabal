Friend Class MoonPortalDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.MoonPortal,,,,,,
            5000,
            MakeList(ShoppeType.BlackMage),,,,
            "IsInDungeon",
            "UseMoonPortal")
    End Sub
End Class
