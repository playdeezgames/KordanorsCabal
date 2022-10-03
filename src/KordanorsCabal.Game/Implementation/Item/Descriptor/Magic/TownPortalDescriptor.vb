Friend Class TownPortalDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.TownPortal,,
            "IsInDungeon")
    End Sub
End Class
