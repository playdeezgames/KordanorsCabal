Friend Class DrunkennessDescriptor
    Inherits CharacterStatisticType

    Public Sub New(characterStatisticTypeId As Long)
        MyBase.New(characterStatisticTypeId)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Drunkenness"
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

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return Name
        End Get
    End Property
End Class
