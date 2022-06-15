Public Class Renderer(Of TPixel As Structure)
    ReadOnly Property FrameBuffer As FrameBuffer(Of TPixel)
    ReadOnly Property PatternBuffer As PatternBuffer
    ReadOnly Property BorderWidth As Integer
    ReadOnly Property BorderHeight As Integer
    ReadOnly Property CellWidth As Integer
    ReadOnly Property CellHeight As Integer
    Property BorderHue As Hue = Hue.Cyan
    Property ScreenHue As Hue = Hue.White
    ReadOnly Property ColorLookupTable As IReadOnlyDictionary(Of Hue, TPixel)

    Sub New(borderSize As (Integer, Integer), gridSize As (Integer, Integer), cellSize As (Integer, Integer), table As IReadOnlyDictionary(Of Hue, TPixel))
        ColorLookupTable = table
        BorderWidth = borderSize.Item1
        BorderHeight = borderSize.Item2
        CellWidth = cellSize.Item1
        CellHeight = cellSize.Item2
        FrameBuffer = New FrameBuffer(Of TPixel)(borderSize.Item1 * 2 + gridSize.Item1 * cellSize.Item1, borderSize.Item2 * 2 + gridSize.Item2 * cellSize.Item2)
        PatternBuffer = New PatternBuffer(gridSize.Item1, gridSize.Item2)
    End Sub

    Sub Update()
        For y = 0 To FrameBuffer.Rows - 1
            Dim screenY = y - BorderHeight
            Dim screenRow = screenY \ CellHeight
            Dim cellY = screenY Mod CellHeight
            For x = 0 To FrameBuffer.Columns - 1
                If x < BorderWidth OrElse y < BorderHeight OrElse x >= FrameBuffer.Columns - BorderWidth OrElse y >= FrameBuffer.Rows - BorderHeight Then
                    FrameBuffer.Pixel(x, y) = ColorLookupTable(BorderHue)
                    Continue For
                End If
                Dim screenX = x - BorderWidth
                Dim screenColumn = screenX \ CellWidth
                Dim cellX = screenX Mod CellWidth
                Dim cell = PatternBuffer.Cell(screenColumn, screenRow)
                Dim bitmap = PETSCII(cell.Pattern)
                Dim hue = If(cell.Inverted Xor bitmap.Pixel(cellX, cellY), cell.Hue, ScreenHue)
                FrameBuffer.Pixel(x, y) = ColorLookupTable(hue)
            Next
        Next
    End Sub

End Class
