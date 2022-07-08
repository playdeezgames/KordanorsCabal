Friend Class ShortswordUIDescriptor
    Inherits ItemTypeUIDescriptor

    Public Overrides ReadOnly Property DisplayPattern As Pattern?
        Get
            Return Pattern.Minus
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayXY As (Integer, Integer)?
        Get
            Return (15, 15)
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayHue As Hue?
        Get
            Return Hue.Black
        End Get
    End Property
End Class
