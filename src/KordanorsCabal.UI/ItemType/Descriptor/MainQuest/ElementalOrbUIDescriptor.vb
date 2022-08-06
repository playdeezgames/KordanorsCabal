Imports SPLORR.Game

Friend Class ElementalOrbUIDescriptor
    Inherits ItemTypeUIDescriptor
    Public Sub New()
        MyBase.New(Pattern.O, (16, 16), Nothing)
    End Sub

    Private Shared ReadOnly Hues As IReadOnlyList(Of Hue) =
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
