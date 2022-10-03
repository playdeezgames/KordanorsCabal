Friend Class HelmetDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Helmet)
    End Sub
End Class
