Friend Class TownPortalDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.TownPortal,,,,,,,,,
            50,
            MakeList(ShoppeType.BlackMage),,,,
            "IsInDungeon",
            "UseTownPortal")
    End Sub
End Class
