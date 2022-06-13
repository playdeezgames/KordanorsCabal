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
    Public ReadOnly BlackHue As New Color(0, 0, 0)
    Public ReadOnly WhiteHue As New Color(255, 255, 255)
    Public ReadOnly RedHue As New Color(&H77, &H2D, &H26)
    Public ReadOnly CyanHue As New Color(&H85, &HD4, &HDC)
    Public ReadOnly PurpleHue As New Color(&HA8, &H5F, &HB4)
    Public ReadOnly GreenHue As New Color(&H55, &H9E, &H4A)
    Public ReadOnly BlueHue As New Color(&H42, &H34, &H8B)
    Public ReadOnly YellowHue As New Color(&HBD, &HCC, &H71)
    Public ReadOnly OrangeHue As New Color(&HA8, &H73, &H4A)
    Public ReadOnly LightOrangeHue As New Color(&HE9, &HB2, &H87)
    Public ReadOnly PinkHue As New Color(&HB6, &H68, &H62)
    Public ReadOnly LightCyanHue As New Color(&HC5, &HFF, &HFF)
    Public ReadOnly LightPurpleHue As New Color(&HE9, &H9D, &HF5)
    Public ReadOnly LightGreenHue As New Color(&H92, &HDF, &H87)
    Public ReadOnly LightBlueHue As New Color(&H7E, &H70, &HCA)
    Public ReadOnly LightYellowHue As New Color(&HFF, &HFF, &HB0)
    Public ReadOnly Characters As IReadOnlyDictionary(Of Char, (Byte, Byte, Byte, Byte, Byte, Byte, Byte, Byte)) =
        New Dictionary(Of Char, (Byte, Byte, Byte, Byte, Byte, Byte, Byte, Byte)) From
        {
            {"@"c, PETSCII(0)},
            {"A"c, PETSCII(1)},
            {"B"c, PETSCII(2)},
            {"C"c, PETSCII(3)},
            {"D"c, PETSCII(4)},
            {"E"c, PETSCII(5)},
            {"F"c, PETSCII(6)},
            {"G"c, PETSCII(7)},
            {"H"c, PETSCII(8)},
            {"I"c, PETSCII(9)},
            {"J"c, PETSCII(10)},
            {"K"c, PETSCII(11)},
            {"L"c, PETSCII(12)},
            {"M"c, PETSCII(13)},
            {"N"c, PETSCII(14)},
            {"O"c, PETSCII(15)},
            {"P"c, PETSCII(16)},
            {"Q"c, PETSCII(17)},
            {"R"c, PETSCII(18)},
            {"S"c, PETSCII(19)},
            {"T"c, PETSCII(20)},
            {"U"c, PETSCII(21)},
            {"V"c, PETSCII(22)},
            {"W"c, PETSCII(23)},
            {"X"c, PETSCII(24)},
            {"Y"c, PETSCII(25)},
            {"Z"c, PETSCII(26)},
            {"["c, PETSCII(27)}, '{"?"c, PETSCII(28)},(GBP sign)
            {"]"c, PETSCII(29)},
            {"^"c, PETSCII(30)}, '{"O"c, PETSCII(31)}, (<-)
            {" "c, PETSCII(32)},
            {"!"c, PETSCII(33)},
            {""""c, PETSCII(34)},
            {"#"c, PETSCII(35)},
            {"$"c, PETSCII(36)},
            {"%"c, PETSCII(37)},
            {"&"c, PETSCII(38)},
            {"'"c, PETSCII(39)},
            {"("c, PETSCII(40)},
            {")"c, PETSCII(41)},
            {"*"c, PETSCII(42)},
            {"+"c, PETSCII(43)},
            {","c, PETSCII(44)},
            {"-"c, PETSCII(45)},
            {"."c, PETSCII(46)},
            {"/"c, PETSCII(47)},
            {"0"c, PETSCII(48)},
            {"1"c, PETSCII(49)},
            {"2"c, PETSCII(50)},
            {"3"c, PETSCII(51)},
            {"4"c, PETSCII(52)},
            {"5"c, PETSCII(53)},
            {"6"c, PETSCII(54)},
            {"7"c, PETSCII(55)},
            {"8"c, PETSCII(56)},
            {"9"c, PETSCII(57)},
            {":"c, PETSCII(58)},
            {";"c, PETSCII(59)},
            {"<"c, PETSCII(60)},
            {"="c, PETSCII(61)},
            {">"c, PETSCII(62)},
            {"?"c, PETSCII(63)}
        }

    Friend Sub PutCellPattern(pattern As (Byte, Byte, Byte, Byte, Byte, Byte, Byte, Byte), cellXY As (Integer, Integer), colors As (Color, Color))
        PutPattern(pattern, (cellXY.Item1 * CellWidth + BorderWidth, cellXY.Item2 * CellHeight + BorderHeight), colors)
    End Sub

    Friend Sub PutCellCharacter(character As Char, xy As (Integer, Integer), colors As (Color, Color))
        PutCellPattern(Characters(character), xy, colors)
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
