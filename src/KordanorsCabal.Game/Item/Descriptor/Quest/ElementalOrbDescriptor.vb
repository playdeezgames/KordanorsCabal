Friend Class ElementalOrbDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Elemental Orb"
        End Get
    End Property
End Class
