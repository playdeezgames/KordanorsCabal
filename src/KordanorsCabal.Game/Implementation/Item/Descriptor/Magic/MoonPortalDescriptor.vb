Friend Class MoonPortalDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.MoonPortal,,,
            "IsInDungeon",
            "UseMoonPortal")
    End Sub
End Class
