Friend Class BottleDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Empty Bottle"
        End Get
    End Property

    Public Overrides ReadOnly Property Encumbrance As Single
        Get
            Return 1.0!
        End Get
    End Property
End Class
