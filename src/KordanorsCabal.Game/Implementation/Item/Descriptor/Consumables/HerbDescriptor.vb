Friend Class HerbDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Herb)
    End Sub
End Class
