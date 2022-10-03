Friend Class FoodDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Food,,
            "AlwaysTrue",
            "EatFood")
    End Sub
End Class
