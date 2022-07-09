Friend Class RatTailDescriptor
    Inherits ItemTypeDescriptor

    Public Overrides ReadOnly Property SpawnLocationTypes(level As Long) As HashSet(Of LocationType)
        Get
            Return New HashSet(Of LocationType)
        End Get
    End Property

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Rat Tail"
        End Get
    End Property

    Public Shared Widening Operator CType(v As RatTailDescriptor) As RatTailDescriptor
        Throw New NotImplementedException()
    End Operator
End Class
