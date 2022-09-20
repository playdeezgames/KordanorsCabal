Friend Class RottenEggDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RottenEgg,,,,,,,,,,,,,
            "CanUseRottenEgg",
            "UseRottenEgg")
    End Sub
End Class
