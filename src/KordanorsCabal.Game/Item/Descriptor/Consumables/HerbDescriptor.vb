Friend Class HerbDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Herb,,,,,,,,,
            5,
            MakeList(ShoppeType.BlackMage),,,,
            "HasBong",
            "UseHerb")
    End Sub
End Class
