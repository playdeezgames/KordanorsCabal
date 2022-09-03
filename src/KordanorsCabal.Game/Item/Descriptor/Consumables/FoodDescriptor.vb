Friend Class FoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.Food,,,,,,,,,
            2,
            MakeList(ShoppeType.InnKeeper),,,,
            "AlwaysTrue",
            "EatFood")
    End Sub
End Class
