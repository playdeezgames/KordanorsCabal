Friend Class PotionDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Potion,
            ,,,,,,,,
            15,
            MakeList(ShoppeType.Healer),,,,
            Function(character) True,
            "DrinkPotion")
    End Sub
End Class
