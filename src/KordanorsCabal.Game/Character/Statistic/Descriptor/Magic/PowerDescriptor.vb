Friend Class PowerDescriptor
    Inherits CharacterStatisticType

    Public Sub New(characterStatisticTypeId As Long)
        MyBase.New(characterStatisticTypeId)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Power"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "Pow"
        End Get
    End Property
End Class
