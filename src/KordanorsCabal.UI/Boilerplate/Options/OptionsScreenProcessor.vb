Friend Class OptionsScreenProcessor
    Inherits MenuProcessor

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Go Back", Function() UIState.TitleScreen),
                ("Screen Size...", Function() UIState.ScreenSizer),
                ("SFX Volume...", Function() UIState.SfxVolumizer),
                ("MUX Volume...", Function() UIState.MuxVolumizer)
            },
            5,
            UIState.OptionsScreen)
    End Sub

    Public Overrides Sub Initialize()
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteText((7, 0), "Options:", False, Hue.Blue)
    End Sub

    Public Overrides Function HandleRed() As UIState
        Return UIState.TitleScreen
    End Function
End Class
