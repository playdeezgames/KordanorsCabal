Friend Class PotionDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Potion,,
            15,
            MakeList(ShoppeType.Healer),,,,
            "AlwaysTrue",
            "DrinkPotion")
    End Sub
End Class
