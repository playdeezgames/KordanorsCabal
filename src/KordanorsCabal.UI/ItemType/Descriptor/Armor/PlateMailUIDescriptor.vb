Friend Class PlateMailUIDescriptor
    Inherits ItemTypeUIDescriptor

    Public Overrides ReadOnly Property DisplayPattern As Pattern?
        Get
            Return Pattern.Dither
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayXY As (Integer, Integer)?
        Get
            Return (4, 16)
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayHue As Hue?
        Get
            Return Hue.Cyan
        End Get
    End Property
End Class
