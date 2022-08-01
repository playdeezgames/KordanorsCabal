Friend Class ManaDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Mana"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return Name
        End Get
    End Property
End Class
