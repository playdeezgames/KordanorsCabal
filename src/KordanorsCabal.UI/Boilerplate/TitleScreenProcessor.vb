Friend Class TitleScreenProcessor
    Inherits MenuProcessor
    Const StartMenuItem = "Start"
    Const ContinueMenuItem = "Continue"
    Const InstructionsMenuItem = "Instructions"
    Const OptionsMenuItem = "Options"
    Const AboutMenuItem = "About"
    Const QuitMenuItem = "Quit"

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                (StartMenuItem, Function() UIState.TitleScreen),
                (ContinueMenuItem, Function() UIState.TitleScreen),
                (InstructionsMenuItem, Function() UIState.InstructionsScreen),
                (OptionsMenuItem, Function() UIState.OptionsScreen),
                (AboutMenuItem, Function() UIState.AboutScreen),
                (QuitMenuItem, Function() UIState.ConfirmQuit)
            },
            14,
            UIState.TitleScreen)
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.FillCells((0, 0), (buffer.Columns, 5), Pattern.Asterisk, True, Hue.Blue)
        buffer.FillCells((1, 1), (buffer.Columns - 2, 3), Pattern.Space, True, Hue.Blue)
        buffer.WriteText((0, 2), "*  Kordanor's Cabal  *", True, Hue.Blue)
        buffer.WriteText((0, 6), "A Game in VB.NET About", False, Hue.Black)
        buffer.WriteText((0, 7), "Looking Like a Dungeon", False, Hue.Black)
        buffer.WriteText((0, 8), " Crawler Written  for ", False, Hue.Black)
        buffer.WriteText((0, 9), "      the VIC-20      ", False, Hue.Black)
        buffer.WriteText((0, 11), "   A Production  of   ", False, Hue.Black)
        buffer.WriteText((0, 12), "   TheGrumpyGameDev   ", False, Hue.Black)
        buffer.WriteText((0, buffer.Rows - 1), "Controls: Arrows/Space", False, Hue.Blue)
    End Sub
End Class
