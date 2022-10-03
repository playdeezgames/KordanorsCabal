Friend Class BottleDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Bottle,,,
            "CanUseBottle",
            "UseBotttle")
    End Sub
End Class
