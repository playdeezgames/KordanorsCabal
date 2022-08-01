Friend Class FoodPoisoningDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Food Poisoning"
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return Name
        End Get
    End Property
End Class
