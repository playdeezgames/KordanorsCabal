Friend Class ElderDescriptor
    Inherits FeatureTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Zooperdan the Elder"
        End Get
    End Property

    Public Overrides ReadOnly Property LocationType As LocationType
        Get
            Return LocationType.TownSquare
        End Get
    End Property
End Class
