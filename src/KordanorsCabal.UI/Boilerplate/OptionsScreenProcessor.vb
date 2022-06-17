Friend Class OptionsScreenProcessor
    Inherits MenuProcessor

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Go Back", Function() UIState.TitleScreen),
                ("Screen Size...", Function() UIState.OptionsScreen)
            },
            5,
            UIState.OptionsScreen)
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteText((0, 0), "Options:", False, Hue.Blue)
    End Sub
End Class
