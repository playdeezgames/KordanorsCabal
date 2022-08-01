Friend Class StrengthDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Strength"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "Str"
        End Get
    End Property
End Class
