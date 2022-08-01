Friend Class MoneyDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Money"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "$"
        End Get
    End Property
End Class
