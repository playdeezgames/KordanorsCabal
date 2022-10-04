Friend Class RottenFoodDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RottenFood,
            "PurifyFood")
    End Sub
End Class
