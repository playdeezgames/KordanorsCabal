Friend Class SkullFragmentDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Skull Fragment"
        End Get
    End Property
End Class
