Friend Class MpDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "MP"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return Name
        End Get
    End Property
End Class
