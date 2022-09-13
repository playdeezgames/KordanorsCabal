Friend Class ElementalOrbDescriptor
    Inherits ItemType

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            OldItemType.ElementalOrb)
    End Sub
End Class
