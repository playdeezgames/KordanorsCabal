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
    End Sub
End Class
