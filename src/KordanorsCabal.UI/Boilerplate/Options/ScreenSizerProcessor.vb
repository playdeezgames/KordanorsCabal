Public Class ScreenSizerProcessor
    Inherits MenuProcessor

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("(1x) 416x240", Function()
                                     SetCurrentScreenSize.Invoke(1)
                                     Return UIState.OptionsScreen
                                 End Function),
                ("(2x) 832x480", Function()
                                     SetCurrentScreenSize.Invoke(2)
                                     Return UIState.OptionsScreen
                                 End Function),
                ("(3x) 1248x720", Function()
                                      SetCurrentScreenSize.Invoke(3)
                                      Return UIState.OptionsScreen
                                  End Function),
                ("(4x) 1664x960", Function()
                                      SetCurrentScreenSize.Invoke(4)
                                      Return UIState.OptionsScreen
                                  End Function),
                ("(5x) 2080x1200", Function()
                                       SetCurrentScreenSize.Invoke(5)
                                       Return UIState.OptionsScreen
                                   End Function),
                ("(6x) 2496x1440", Function()
                                       SetCurrentScreenSize.Invoke(6)
                                       Return UIState.OptionsScreen
                                   End Function),
                ("(7x) 2912x1680", Function()
                                       SetCurrentScreenSize.Invoke(7)
                                       Return UIState.OptionsScreen
                                   End Function),
                ("(8x) 3328x1920", Function()
                                       SetCurrentScreenSize.Invoke(8)
                                       Return UIState.OptionsScreen
                                   End Function)
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
