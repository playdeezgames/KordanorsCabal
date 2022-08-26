Friend Class ElementalOrbDescriptor
    Inherits ItemTypeDescriptor

    Sub New()
        MyBase.New(
            StaticWorldData.World,
            ItemType.ElementalOrb,
            "Elemental Orb")
    End Sub
End Class
