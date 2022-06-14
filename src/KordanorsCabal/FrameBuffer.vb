Imports Microsoft.Xna.Framework
Imports Microsoft.Xna.Framework.Graphics

Public Class FrameBuffer
    ReadOnly Property Columns As Integer
    ReadOnly Property Rows As Integer
    Private ReadOnly _pixels() As Color
    Property Pixel(x As Integer, y As Integer) As Color
        Get
            Return _pixels(x + y * Columns)
        End Get
        Set(value As Color)
            _pixels(x + y * Columns) = value
        End Set
    End Property
    Sub New(columns As Integer, rows As Integer)
        Me.Columns = columns
        Me.Rows = rows
        ReDim _pixels(columns * rows - 1)
    End Sub
    Sub UpdateTexture(texture As Texture2D)
        texture.SetData(_pixels)
    End Sub
End Class
