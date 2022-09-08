Friend Class IronKeyDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.IronKey)
    End Sub
End Class
