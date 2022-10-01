Friend Class BottleDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Bottle,,
            3,,,,,,,,
            "CanUseBottle",
            "UseBotttle")
    End Sub
End Class
