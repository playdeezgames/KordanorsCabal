Friend Class PotionDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Potion,,
            MakeList(ShoppeType.Healer),,,,
            "AlwaysTrue",
            "DrinkPotion")
    End Sub
End Class
