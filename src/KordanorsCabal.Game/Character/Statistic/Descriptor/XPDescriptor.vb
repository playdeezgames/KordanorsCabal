Friend Class XPDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "XP"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return Name
        End Get
    End Property
End Class
