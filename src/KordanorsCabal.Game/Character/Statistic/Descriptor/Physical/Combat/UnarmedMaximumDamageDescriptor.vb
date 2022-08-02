Friend Class UnarmedMaximumDamageDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Unarmed Maximum Damage"
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultValue As Long?
        Get
            Return 1
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "MaxDmg"
        End Get
    End Property
End Class
