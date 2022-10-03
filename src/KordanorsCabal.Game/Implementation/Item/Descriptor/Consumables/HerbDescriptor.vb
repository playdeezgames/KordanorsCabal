Friend Class HerbDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Herb,,
            "HasBong",
            "UseHerb")
    End Sub
End Class
