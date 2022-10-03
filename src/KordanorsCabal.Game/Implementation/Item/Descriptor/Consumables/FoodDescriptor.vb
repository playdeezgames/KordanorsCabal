Friend Class FoodDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Food,
            MakeList(ShoppeType.InnKeeper),,,
            "AlwaysTrue",
            "EatFood")
    End Sub
End Class
