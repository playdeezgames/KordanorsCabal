Friend Class BottleDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Bottle,,
            "CanUseBottle")
    End Sub
End Class
