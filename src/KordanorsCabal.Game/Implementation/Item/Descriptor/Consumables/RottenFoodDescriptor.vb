Friend Class RottenFoodDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RottenFood,,,,,,,,,
            "PurifyFood",
            "AlwaysTrue",
            "UseRottenFood")
    End Sub
End Class
