Public Class ScreenSizerProcessor
    Inherits MenuProcessor
    Private Shared Function MakeMenuItem(size As Integer) As (String, Func(Of UIState))
        Return ($"({size}x) {size * 416}x{size * 240}", Function()
                                                            SetCurrentScreenSize.Invoke(size)
                                                            Return UIState.OptionsScreen
                                                        End Function)
    End Function

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                MakeMenuItem(1),
                MakeMenuItem(2),
                MakeMenuItem(3),
                MakeMenuItem(4),
                MakeMenuItem(5),
                MakeMenuItem(6),
                MakeMenuItem(7),
                MakeMenuItem(8)
            },
            5,
            UIState.ScreenSizer)
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteText((5, 0), "Screen Size:", False, Hue.Blue)
    End Sub

    Public Overrides Sub Initialize()

        currentItem = GetCurrentScreenSize.Invoke() - 1
    End Sub

    Public Shared Property GetCurrentScreenSize As Func(Of Integer)
    Public Shared Property SetCurrentScreenSize As Action(Of Integer)
    Public Overrides Function HandleRed() As UIState
        Return UIState.OptionsScreen
    End Function
End Class
