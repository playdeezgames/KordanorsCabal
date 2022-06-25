Friend Class ElementalOrbDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property
End Class
