Imports Microsoft.Xna.Framework
Module FrameBuffer
    Public Const BorderWidth = 16
    Public Const BorderHeight = 28
    Public Const CellWidth = 8
    Public Const CellHeight = 8
    Public Const CellColumns = 22
    Public Const CellRows = 23
    Public Const ViewWidth = CellWidth * CellColumns + BorderWidth * 2
    Public Const ViewHeight = CellHeight * CellRows + BorderHeight * 2
    Public FrameData(ViewWidth * ViewHeight - 1) As Color

    Friend Sub PutCellPattern(pattern As Pattern, cellXY As (Integer, Integer), colors As (Color, Color))
        PutPattern(PETSCII(pattern), (cellXY.Item1 * CellWidth + BorderWidth, cellXY.Item2 * CellHeight + BorderHeight), colors)
    End Sub

    Friend Sub PutCellCharacter(character As Char, xy As (Integer, Integer), colors As (Color, Color))
        PutCellPattern(CharacterPattern(character), xy, colors)
    End Sub

    Friend Sub WriteString(text As String, xy As (Integer, Integer), colors As (Color, Color))
        For Each character In text
            PutCellCharacter(character, xy, colors)
            xy = AdvanceXY(xy)
        Next
    End Sub

    Private Function AdvanceXY(xy As (Integer, Integer)) As (Integer, Integer)
        Dim nextX = xy.Item1 + 1
        Dim nextY = xy.Item2
        If nextX >= CellColumns Then
            nextY += 1
            nextX = 0
            If nextY >= CellRows Then
                nextY = 0
            End If
        End If
        Return (nextX, nextY)
    End Function

    Sub ClearBorder(color As Color)
        For x = 0 To ViewWidth - 1
            For y = 0 To ViewHeight - 1
                If x < BorderWidth OrElse y < BorderHeight OrElse x >= ViewWidth - BorderWidth OrElse y >= ViewHeight - BorderHeight Then
                    SetFramePixel(x, y, color)
                End If
            Next
        Next
    End Sub
    Sub ClearScreen(color As Color)
        For x = BorderWidth To ViewWidth - BorderWidth - 1
            For y = BorderHeight To ViewHeight - BorderHeight - 1
                SetFramePixel(x, y, color)
            Next
        Next
    End Sub

    Private Sub SetFramePixel(x As Integer, y As Integer, color As Color)
        FrameData(x + y * ViewWidth) = color
    End Sub

    Private Sub PutPattern(pattern As (Byte, Byte, Byte, Byte, Byte, Byte, Byte, Byte), xy As (Integer, Integer), colors As (Color, Color))
        PutPatternLine(pattern.Item1, (xy.Item1, xy.Item2), colors)
        PutPatternLine(pattern.Item2, (xy.Item1, xy.Item2 + 1), colors)
        PutPatternLine(pattern.Item3, (xy.Item1, xy.Item2 + 2), colors)
        PutPatternLine(pattern.Item4, (xy.Item1, xy.Item2 + 3), colors)
        PutPatternLine(pattern.Item5, (xy.Item1, xy.Item2 + 4), colors)
        PutPatternLine(pattern.Item6, (xy.Item1, xy.Item2 + 5), colors)
        PutPatternLine(pattern.Item7, (xy.Item1, xy.Item2 + 6), colors)
        PutPatternLine(pattern.Item8, (xy.Item1, xy.Item2 + 7), colors)
    End Sub

    Private Sub PutPatternLine(characterLine As Byte, xy As (Integer, Integer), colors As (Color, Color))
        If (characterLine And 1) > 0 Then
            PutPixel(xy, colors.Item1)
        Else
            PutPixel(xy, colors.Item2)
        End If
        If (characterLine And 2) > 0 Then
            PutPixel((xy.Item1 + 1, xy.Item2), colors.Item1)
        Else
            PutPixel((xy.Item1 + 1, xy.Item2), colors.Item2)
        End If
        If (characterLine And 4) > 0 Then
            PutPixel((xy.Item1 + 2, xy.Item2), colors.Item1)
        Else
            PutPixel((xy.Item1 + 2, xy.Item2), colors.Item2)
        End If
        If (characterLine And 8) > 0 Then
            PutPixel((xy.Item1 + 3, xy.Item2), colors.Item1)
        Else
            PutPixel((xy.Item1 + 3, xy.Item2), colors.Item2)
        End If
        If (characterLine And 16) > 0 Then
            PutPixel((xy.Item1 + 4, xy.Item2), colors.Item1)
        Else
            PutPixel((xy.Item1 + 4, xy.Item2), colors.Item2)
        End If
        If (characterLine And 32) > 0 Then
            PutPixel((xy.Item1 + 5, xy.Item2), colors.Item1)
        Else
            PutPixel((xy.Item1 + 5, xy.Item2), colors.Item2)
        End If
        If (characterLine And 64) > 0 Then
            PutPixel((xy.Item1 + 6, xy.Item2), colors.Item1)
        Else
            PutPixel((xy.Item1 + 6, xy.Item2), colors.Item2)
        End If
        If (characterLine And 128) > 0 Then
            PutPixel((xy.Item1 + 7, xy.Item2), colors.Item1)
        Else
            PutPixel((xy.Item1 + 7, xy.Item2), colors.Item2)
        End If
    End Sub

    Private Sub PutPixel(xy As (Integer, Integer), color As Color)
        FrameData(xy.Item1 + xy.Item2 * ViewWidth) = color
    End Sub
End Module
