Imports Microsoft.Xna.Framework
Module FrameBuffer
    Public Const ViewWidth = 208
    Public Const ViewHeight = 240
    Public Const BorderWidth = 16
    Public Const BorderHeight = 28
    Public FrameData(ViewWidth * ViewHeight - 1) As Color
    Public ReadOnly BlackHue As Color = New Color(0, 0, 0)
    Public ReadOnly WhiteHue As Color = New Color(255, 255, 255)
    Public ReadOnly OrangeHue As Color = New Color(&HA8, &H73, &H4A)
    Public ReadOnly LightOrangeHue As Color = New Color(&HE9, &HB2, &H87)

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
End Module
