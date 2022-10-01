Friend Class TownPortalDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.TownPortal,,,,
            50,
            MakeList(ShoppeType.BlackMage),,,,
            "IsInDungeon",
            "UseTownPortal")
    End Sub
End Class
