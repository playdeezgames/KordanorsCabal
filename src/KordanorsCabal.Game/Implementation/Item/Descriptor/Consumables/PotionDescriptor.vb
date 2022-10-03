Friend Class PotionDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Potion,,
            "AlwaysTrue")
    End Sub
End Class
