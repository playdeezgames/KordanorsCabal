Public Class PatternBitmap
    Private ReadOnly _lines As List(Of Byte)
    Sub New(line0 As Byte, line1 As Byte, line2 As Byte, line3 As Byte, line4 As Byte, line5 As Byte, line6 As Byte, line7 As Byte)
        _lines = New List(Of Byte) From {line0, line1, line2, line3, line4, line5, line6, line7}
    End Sub
    Private Shared ReadOnly ColumnMasks As IReadOnlyList(Of Byte) =
        New List(Of Byte) From {1, 2, 4, 8, 16, 32, 64, 128}
    ReadOnly Property Pixel(column As Integer, row As Integer) As Boolean
        Get
            If column < 0 OrElse column >= 8 OrElse row < 0 OrElse row >= 8 Then
                Return False
            End If
            Return (_lines(row) And ColumnMasks(column)) > 0
        End Get
    End Property
End Class
Public Module Patterns
    'VIC20:
    'ASPECT_RATIO: 2:1
    'SCREEN_WIDTH: 208
    'SCREEN_HEIGHT: 240
    'VIEW_OFFSET_X: 16
    'VIEW_OFFSET_Y: 28
    'VIEW_WIDTH: 176
    'VIEW_HEIGHT: 184
    'CELL_COLUMNS: 22
    'CELL_ROWS: 23
    'CELL_WIDTH: 8
    'CELL_HEIGHT: 8
    Public ReadOnly PETSCII As IReadOnlyList(Of PatternBitmap) =
        New List(Of PatternBitmap) From
        {
            New PatternBitmap(56, 68, 82, 106, 50, 4, 120, 0),
            New PatternBitmap(24, 36, 66, 126, 66, 66, 66, 0),
            New PatternBitmap(62, 68, 68, 60, 68, 68, 62, 0),
            New PatternBitmap(56, 68, 2, 2, 2, 68, 56, 0),
            New PatternBitmap(30, 36, 68, 68, 68, 36, 30, 0),
            New PatternBitmap(126, 2, 2, 30, 2, 2, 126, 0),
            New PatternBitmap(126, 2, 2, 30, 2, 2, 2, 0),
            New PatternBitmap(56, 68, 2, 114, 66, 68, 56, 0),
            New PatternBitmap(66, 66, 66, 126, 66, 66, 66, 0),
            New PatternBitmap(56, 16, 16, 16, 16, 16, 56, 0),
            New PatternBitmap(112, 32, 32, 32, 32, 34, 28, 0),
            New PatternBitmap(66, 34, 18, 14, 18, 34, 66, 0),
            New PatternBitmap(2, 2, 2, 2, 2, 2, 126, 0),
            New PatternBitmap(66, 102, 90, 90, 66, 66, 66, 0),
            New PatternBitmap(66, 70, 74, 82, 98, 66, 66, 0),
            New PatternBitmap(24, 36, 66, 66, 66, 36, 24, 0),
            New PatternBitmap(62, 66, 66, 62, 2, 2, 2, 0),
            New PatternBitmap(24, 36, 66, 66, 82, 36, 88, 0),
            New PatternBitmap(62, 66, 66, 62, 18, 34, 66, 0),
            New PatternBitmap(60, 66, 2, 60, 64, 66, 60, 0),
            New PatternBitmap(124, 16, 16, 16, 16, 16, 16, 0),
            New PatternBitmap(66, 66, 66, 66, 66, 66, 60, 0),
            New PatternBitmap(66, 66, 66, 36, 36, 24, 24, 0),
            New PatternBitmap(66, 66, 66, 90, 90, 102, 66, 0),
            New PatternBitmap(66, 66, 36, 24, 36, 66, 66, 0),
            New PatternBitmap(68, 68, 68, 56, 16, 16, 16, 0),
            New PatternBitmap(126, 64, 32, 24, 4, 2, 126, 0),
            New PatternBitmap(60, 4, 4, 4, 4, 4, 60, 0),
            New PatternBitmap(48, 8, 8, 60, 8, 14, 118, 0),
            New PatternBitmap(60, 32, 32, 32, 32, 32, 60, 0),
            New PatternBitmap(0, 16, 56, 84, 16, 16, 16, 16),
            New PatternBitmap(0, 0, 8, 4, 254, 4, 8, 0),
            New PatternBitmap(0, 0, 0, 0, 0, 0, 0, 0),
            New PatternBitmap(16, 16, 16, 16, 0, 0, 16, 0),
            New PatternBitmap(36, 36, 36, 0, 0, 0, 0, 0),
            New PatternBitmap(36, 36, 126, 36, 126, 36, 36, 0),
            New PatternBitmap(16, 120, 20, 56, 80, 60, 16, 0),
            New PatternBitmap(0, 70, 38, 16, 8, 100, 98, 0),
            New PatternBitmap(12, 18, 18, 12, 82, 34, 92, 0),
            New PatternBitmap(32, 16, 8, 0, 0, 0, 0, 0),
            New PatternBitmap(32, 16, 8, 8, 8, 16, 32, 0),
            New PatternBitmap(4, 8, 16, 16, 16, 8, 4, 0),
            New PatternBitmap(16, 84, 56, 124, 56, 84, 16, 0),
            New PatternBitmap(0, 16, 16, 124, 16, 16, 0, 0),
            New PatternBitmap(0, 0, 0, 0, 0, 16, 16, 8),
            New PatternBitmap(0, 0, 0, 126, 0, 0, 0, 0),
            New PatternBitmap(0, 0, 0, 0, 0, 24, 24, 0),
            New PatternBitmap(0, 64, 32, 16, 8, 4, 2, 0),
            New PatternBitmap(60, 66, 98, 90, 70, 66, 60, 0),
            New PatternBitmap(16, 24, 20, 16, 16, 16, 124, 0),
            New PatternBitmap(60, 66, 64, 48, 12, 2, 126, 0),
            New PatternBitmap(60, 66, 64, 56, 64, 66, 60, 0),
            New PatternBitmap(32, 48, 40, 36, 126, 32, 32, 0),
            New PatternBitmap(126, 2, 30, 32, 64, 34, 28, 0),
            New PatternBitmap(56, 4, 2, 62, 66, 66, 60, 0),
            New PatternBitmap(126, 66, 32, 16, 8, 8, 8, 0),
            New PatternBitmap(60, 66, 66, 60, 66, 66, 60, 0),
            New PatternBitmap(60, 66, 66, 124, 64, 32, 28, 0),
            New PatternBitmap(0, 0, 16, 0, 0, 16, 0, 0),
            New PatternBitmap(0, 0, 16, 0, 0, 16, 16, 8),
            New PatternBitmap(112, 24, 12, 6, 12, 24, 112, 0),
            New PatternBitmap(0, 0, 126, 0, 126, 0, 0, 0),
            New PatternBitmap(14, 24, 48, 96, 48, 24, 14, 0),
            New PatternBitmap(60, 66, 64, 48, 8, 0, 8, 0),
            New PatternBitmap(0, 0, 0, 0, 255, 0, 0, 0),
            New PatternBitmap(16, 56, 124, 254, 254, 56, 124, 0),
            New PatternBitmap(8, 8, 8, 8, 8, 8, 8, 8),
            New PatternBitmap(0, 0, 0, 255, 0, 0, 0, 0),
            New PatternBitmap(0, 0, 255, 0, 0, 0, 0, 0),
            New PatternBitmap(0, 255, 0, 0, 0, 0, 0, 0),
            New PatternBitmap(0, 0, 0, 0, 0, 255, 0, 0),
            New PatternBitmap(4, 4, 4, 4, 4, 4, 4, 4),
            New PatternBitmap(32, 32, 32, 32, 32, 32, 32, 32),
            New PatternBitmap(0, 0, 0, 0, 7, 8, 16, 16),
            New PatternBitmap(16, 16, 16, 32, 192, 0, 0, 0),
            New PatternBitmap(16, 16, 16, 8, 7, 0, 0, 0),
            New PatternBitmap(1, 1, 1, 1, 1, 1, 1, 255),
            New PatternBitmap(1, 2, 4, 8, 16, 32, 64, 128),
            New PatternBitmap(128, 64, 32, 16, 8, 4, 2, 1),
            New PatternBitmap(255, 1, 1, 1, 1, 1, 1, 1),
            New PatternBitmap(255, 128, 128, 128, 128, 128, 128, 128),
            New PatternBitmap(0, 60, 126, 126, 126, 126, 60, 0),
            New PatternBitmap(0, 0, 0, 0, 0, 0, 255, 0),
            New PatternBitmap(108, 254, 254, 254, 124, 56, 16, 0),
            New PatternBitmap(2, 2, 2, 2, 2, 2, 2, 2),
            New PatternBitmap(0, 0, 0, 0, 192, 32, 16, 16),
            New PatternBitmap(129, 66, 36, 24, 24, 36, 66, 129),
            New PatternBitmap(0, 60, 66, 66, 66, 66, 60, 0),
            New PatternBitmap(16, 56, 84, 238, 84, 16, 16, 0),
            New PatternBitmap(64, 64, 64, 64, 64, 64, 64, 64),
            New PatternBitmap(16, 56, 124, 254, 124, 56, 16, 0),
            New PatternBitmap(16, 16, 16, 16, 255, 16, 16, 16),
            New PatternBitmap(5, 10, 5, 10, 5, 10, 5, 10),
            New PatternBitmap(16, 16, 16, 16, 16, 16, 16, 16),
            New PatternBitmap(0, 0, 128, 124, 42, 40, 40, 0),
            New PatternBitmap(255, 254, 252, 248, 240, 224, 192, 128),
            New PatternBitmap(0, 0, 0, 0, 0, 0, 0, 0),
            New PatternBitmap(15, 15, 15, 15, 15, 15, 15, 15),
            New PatternBitmap(0, 0, 0, 0, 255, 255, 255, 255),
            New PatternBitmap(255, 0, 0, 0, 0, 0, 0, 0),
            New PatternBitmap(0, 0, 0, 0, 0, 0, 0, 255),
            New PatternBitmap(1, 1, 1, 1, 1, 1, 1, 1),
            New PatternBitmap(85, 170, 85, 170, 85, 170, 85, 170),
            New PatternBitmap(128, 128, 128, 128, 128, 128, 128, 128),
            New PatternBitmap(0, 0, 0, 0, 85, 170, 85, 170),
            New PatternBitmap(255, 127, 63, 31, 15, 7, 3, 1),
            New PatternBitmap(192, 192, 192, 192, 192, 192, 192, 192),
            New PatternBitmap(16, 16, 16, 16, 240, 16, 16, 16),
            New PatternBitmap(0, 0, 0, 0, 240, 240, 240, 240),
            New PatternBitmap(16, 16, 16, 16, 240, 0, 0, 0),
            New PatternBitmap(0, 0, 0, 0, 31, 16, 16, 16),
            New PatternBitmap(0, 0, 0, 0, 0, 0, 255, 255),
            New PatternBitmap(0, 0, 0, 0, 240, 16, 16, 16),
            New PatternBitmap(16, 16, 16, 16, 255, 0, 0, 0),
            New PatternBitmap(0, 0, 0, 0, 255, 16, 16, 16),
            New PatternBitmap(16, 16, 16, 16, 31, 16, 16, 16),
            New PatternBitmap(3, 3, 3, 3, 3, 3, 3, 3),
            New PatternBitmap(7, 7, 7, 7, 7, 7, 7, 7),
            New PatternBitmap(224, 224, 224, 224, 224, 224, 224, 224),
            New PatternBitmap(255, 255, 0, 0, 0, 0, 0, 0),
            New PatternBitmap(255, 255, 255, 0, 0, 0, 0, 0),
            New PatternBitmap(0, 0, 0, 0, 0, 255, 255, 255),
            New PatternBitmap(128, 128, 128, 128, 128, 128, 128, 255),
            New PatternBitmap(0, 0, 0, 0, 15, 15, 15, 15),
            New PatternBitmap(240, 240, 240, 240, 0, 0, 0, 0),
            New PatternBitmap(16, 16, 16, 16, 31, 0, 0, 0),
            New PatternBitmap(15, 15, 15, 15, 0, 0, 0, 0),
            New PatternBitmap(15, 15, 15, 15, 240, 240, 240, 240)
        }
End Module
