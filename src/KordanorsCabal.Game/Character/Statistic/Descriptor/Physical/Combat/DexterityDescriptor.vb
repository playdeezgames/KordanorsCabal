Friend Class DexterityDescriptor
    Inherits CharacterStatisticType

    Public Sub New(characterStatisticTypeId As Long)
        MyBase.New(characterStatisticTypeId)
    End Sub

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Dexterity"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "Dex"
        End Get
    End Property
End Class
