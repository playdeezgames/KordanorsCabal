Friend Class TitleScreenProcessor
    Implements IProcessor
    Const StartMenuItem = "Start"
    Const ContinueMenuItem = "Continue"
    Const InstructionsMenuItem = "Instructions"
    Const OptionsMenuItem = "Options"
    Const AboutMenuItem = "About"
    Const QuitMenuItem = "Quit"

    ReadOnly MenuItems As IReadOnlyList(Of (String, Func(Of UIState))) =
        New List(Of (String, Func(Of UIState))) From
        {
            (StartMenuItem, Function() UIState.TitleScreen),
            (ContinueMenuItem, Function() UIState.TitleScreen),
            (InstructionsMenuItem, Function() UIState.TitleScreen),
            (OptionsMenuItem, Function() UIState.TitleScreen),
            (AboutMenuItem, Function() UIState.AboutScreen),
            (QuitMenuItem, Function() UIState.ConfirmQuit)
        }
    Const MenuRow = 14

    Private currentItem As Integer = 0

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
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
        Dim index As Integer = 0
        For Each menuItem In MenuItems
            buffer.WriteText(((buffer.Columns - menuItem.Item1.Length) \ 2, MenuRow + index), menuItem.Item1, index = currentItem, Hue.Orange)
            index += 1
        Next
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Up
                PreviousMenuItem()
            Case Command.Down
                NextMenuItem()
            Case Command.Green, Command.Blue
                Return MenuItems(currentItem).Item2.Invoke
        End Select
        Return UIState.TitleScreen
    End Function

    Private Sub NextMenuItem()
        currentItem = (currentItem + 1) Mod MenuItems.Count
    End Sub

    Private Sub PreviousMenuItem()
        currentItem = (currentItem + MenuItems.Count - 1) Mod MenuItems.Count
    End Sub
End Class
