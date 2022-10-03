Friend Class TownPortalDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.TownPortal,,
            "IsInDungeon",
            "UseTownPortal")
    End Sub
End Class
