﻿Friend Class MagicEggUIDescriptor
    Inherits ItemTypeUIDescriptor

    Public Overrides ReadOnly Property DisplayPattern As Pattern?
        Get
            Return Pattern.EmptyCircle
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayXY As (Integer, Integer)?
        Get
            Return (8, 15)
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayHue As Hue?
        Get
            Return Hue.Black
        End Get
    End Property
End Class