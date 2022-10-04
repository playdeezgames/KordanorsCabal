Friend Class TownPortalDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.TownPortal)
    End Sub
End Class
