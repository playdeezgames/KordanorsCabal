Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Potion,
            ,,,,,,,,
            15,
            MakeList(ShoppeType.Healer),,,,
            "AlwaysTrue",
            "DrinkPotion")
    End Sub
End Class
