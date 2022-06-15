Public Class PatternBuffer
    ReadOnly Property Columns As Integer
    ReadOnly Property Rows As Integer
    Private ReadOnly _cells() As PatternCell
    ReadOnly Property Cell(column As Integer, row As Integer) As PatternCell
        Get
            Return _cells(column + row * Columns)
        End Get
    End Property
    Sub New(columns As Integer, rows As Integer)
        Me.Columns = columns
        Me.Rows = rows
        ReDim _cells(columns * rows - 1)
        For index = 0 To columns * rows - 1
            _cells(index) = New PatternCell
        Next
    End Sub
    Sub PutCell(xy As (Integer, Integer), pattern As Pattern, inverted As Boolean, hue As Hue)
        Cell(xy.Item1, xy.Item2).Pattern = pattern
        Cell(xy.Item1, xy.Item2).Inverted = inverted
        Cell(xy.Item1, xy.Item2).Hue = hue
    End Sub
    Sub FillCells(xy As (Integer, Integer), wh As (Integer, Integer), pattern As Pattern, inverted As Boolean, hue As Hue)
        For x = xy.Item1 To xy.Item1 + wh.Item1 - 1
            For y = xy.Item2 To xy.Item2 + wh.Item2 - 1
                PutCell((x, y), pattern, inverted, hue)
            Next
        Next
    End Sub
    Sub Fill(pattern As Pattern, inverted As Boolean, hue As Hue)
        FillCells((0, 0), (Columns, Rows), pattern, inverted, hue)
    End Sub
    Sub WriteText(xy As (Integer, Integer), text As String, inverted As Boolean, hue As Hue)
        For Each character In text
            PutCell(xy, CharacterPattern(character), inverted, hue)
            Dim nextX = xy.Item1 + 1
            Dim nextY = xy.Item2
            If nextX >= Columns Then
                nextX = 0
                nextY += 1
            End If
            If nextY >= Rows Then
                nextY = 0
            End If
            xy = (nextX, nextY)
        Next
    End Sub
End Class
