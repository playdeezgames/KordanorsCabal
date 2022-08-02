Friend Class HungerDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Hunger"
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultValue As Long?
        Get
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property MinimumValue As Long
        Get
            Return 0
        End Get
    End Property

    Public Overrides ReadOnly Property MaximumValue As Long
        Get
            Return 100
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return Name
        End Get
    End Property
End Class
