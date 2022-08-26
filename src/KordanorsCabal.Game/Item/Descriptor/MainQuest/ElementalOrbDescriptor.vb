Friend Class ElementalOrbDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.ElementalOrb)
    End Sub
End Class
