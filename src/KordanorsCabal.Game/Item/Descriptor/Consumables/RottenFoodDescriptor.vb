Friend Class RottenFoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.RottenFood,,,,,,,,,,,,,
            "PurifyFood",
            "AlwaysTrue",
            "UseRottenFood")
    End Sub
End Class
