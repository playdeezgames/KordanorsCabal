Public Class Sprite
    ReadOnly Property Hue As Hue
    ReadOnly Property Pixels As IReadOnlyDictionary(Of (Integer, Integer), (Pattern, Boolean))
    Sub New(hue As Hue, pixels As IReadOnlyDictionary(Of (Integer, Integer), (Pattern, Boolean)))
        Me.Hue = hue
        Me.Pixels = pixels
    End Sub
End Class
