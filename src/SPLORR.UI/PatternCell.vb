Public Class PatternCell
    Property Pattern As Pattern
    Property Hue As Hue
    Property Inverted As Boolean
    Property Value As Byte
        Get
            Return CByte(If(Inverted, 128, 0)) + CByte(Pattern)
        End Get
        Set(value As Byte)
            Inverted = value >= 128
            Pattern = CType(value Mod 128, Pattern)
        End Set
    End Property
End Class
