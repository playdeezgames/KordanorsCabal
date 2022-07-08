Imports SPLORR.Game

Friend Class ElementalOrbUIDescriptor
    Inherits ItemTypeUIDescriptor

    Public Overrides ReadOnly Property DisplayPattern As Pattern?
        Get
            Return Pattern.O
        End Get
    End Property

    Public Overrides ReadOnly Property DisplayXY As (Integer, Integer)?
        Get
            Return (16, 16)
        End Get
    End Property

    Private Shared Hues As IReadOnlyList(Of Hue) =
        New List(Of Hue) From
        {
            Hue.Red,
            Hue.Blue,
            Hue.Black,
            Hue.Cyan
        }

    Public Overrides ReadOnly Property DisplayHue As Hue?
        Get
            Return RNG.FromEnumerable(Hues)
        End Get
    End Property
End Class
