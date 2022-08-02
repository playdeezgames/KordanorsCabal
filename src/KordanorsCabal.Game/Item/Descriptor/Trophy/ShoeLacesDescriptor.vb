Friend Class ShoeLacesDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType) From {LocationType.Moon}
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Shoe Laces"
        End Get
    End Property
End Class
