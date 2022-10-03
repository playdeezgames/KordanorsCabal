Friend Class HerbDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Herb,,
            "HasBong")
    End Sub
End Class
