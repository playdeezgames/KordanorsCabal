Friend Class ShieldDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.Shield)
    End Sub
End Class
