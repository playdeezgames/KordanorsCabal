Friend Class ConfirmQuitProcessor
    Inherits MenuProcessor

    Const NoMenuItem = "No"
    Const YesMenuItem = "Yes"
    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                (NoMenuItem, Function() UIState.TitleScreen),
                (YesMenuItem, Function() UIState.None)
            },
            14,
            UIState.ConfirmQuit)
    End Sub

    Public Overrides Sub Initialize()
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteText((0, 10), "Are you sure you want to quit?", False, Hue.Red)
    End Sub

    Public Overrides Function HandleRed() As UIState
        Return UIState.TitleScreen
    End Function
End Class
