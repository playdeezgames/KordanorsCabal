Friend Class RottenFoodDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.RottenFood,,,,,,,,,,,,,
            "PurifyFood",
            Function(character) True,
            "UseRottenFood")
    End Sub
End Class
