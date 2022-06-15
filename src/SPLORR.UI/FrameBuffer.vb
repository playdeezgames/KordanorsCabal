Public Class FrameBuffer(Of TPixel As Structure)
    ReadOnly Property Columns As Integer
    ReadOnly Property Rows As Integer
    Public ReadOnly Pixels() As TPixel
    Property Pixel(x As Integer, y As Integer) As TPixel
        Get
            Return Pixels(x + y * Columns)
        End Get
        Set(value As TPixel)
            Pixels(x + y * Columns) = value
        End Set
    End Property
    Sub New(columns As Integer, rows As Integer)
        Me.Columns = columns
        Me.Rows = rows
        ReDim Pixels(columns * rows - 1)
    End Sub
End Class
