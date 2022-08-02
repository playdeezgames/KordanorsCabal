Friend Class BaseMaximumDefendDescriptor
    Inherits CharacterStatisticTypeDescriptor

    Public Overrides ReadOnly Property Name As String
        Get
            Return "Base Maximum Defend"
        End Get
    End Property

    Public Overrides ReadOnly Property DefaultValue As Long?
        Get
            Return 1
        End Get
    End Property

    Public Overrides ReadOnly Property Abbreviation As String
        Get
            Return "MaxDef"
        End Get
    End Property
End Class
