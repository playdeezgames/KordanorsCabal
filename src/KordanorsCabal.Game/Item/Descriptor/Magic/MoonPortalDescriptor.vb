Friend Class MoonPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.MoonPortal,,,,,,,,,
            5000,
            MakeList(ShoppeType.BlackMage),,,,
            "IsInDungeon",
            "UseMoonPortal")
    End Sub
End Class
