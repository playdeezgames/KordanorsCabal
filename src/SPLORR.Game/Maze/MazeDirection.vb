Public Class MazeDirection(Of TDirection)
    ReadOnly Property Opposite As TDirection
    ReadOnly Property DeltaX As Long
    ReadOnly Property DeltaY As Long
    Sub New(opposite As TDirection, deltaX As Long, deltaY As Long)
        Me.Opposite = opposite
        Me.DeltaX = deltaX
        Me.DeltaY = deltaY
    End Sub
End Class
