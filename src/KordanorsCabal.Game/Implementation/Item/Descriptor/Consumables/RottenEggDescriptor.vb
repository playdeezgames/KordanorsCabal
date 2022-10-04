Friend Class RottenEggDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.RottenEgg)
    End Sub
End Class
