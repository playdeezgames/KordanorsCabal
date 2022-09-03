Friend Class RottenEggDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.RottenEgg,,,,,,,,,,,,,,
            "CanUseRottenEgg",
            "UseRottenEgg")
    End Sub
End Class
