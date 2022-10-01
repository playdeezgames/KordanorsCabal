Friend Class HerbDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Herb,,,,,
            5,
            MakeList(ShoppeType.BlackMage),,,,
            "HasBong",
            "UseHerb")
    End Sub
End Class
