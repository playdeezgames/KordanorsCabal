Friend Class BottleDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Bottle,
            1,,
            3,,,,,,,,
            "CanUseBottle",
            "UseBotttle")
    End Sub
End Class
