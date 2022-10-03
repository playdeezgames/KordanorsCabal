Friend Class RottenEggDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RottenEgg,,
            "CanUseRottenEgg")
    End Sub
End Class
