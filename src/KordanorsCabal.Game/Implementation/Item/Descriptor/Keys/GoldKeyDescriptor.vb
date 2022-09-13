Friend Class GoldKeyDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.GoldKey)
    End Sub
End Class
