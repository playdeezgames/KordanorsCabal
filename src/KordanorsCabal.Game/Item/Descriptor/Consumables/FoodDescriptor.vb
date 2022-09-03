Friend Class FoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Food,,,,,,,,,
            2,
            MakeList(ShoppeType.InnKeeper),,,,
            Function(character) True,
            "EatFood")
    End Sub
End Class
