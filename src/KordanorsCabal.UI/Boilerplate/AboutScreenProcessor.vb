Friend Class AboutScreenProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "About", True, Hue.Blue)
        buffer.WriteText((0, 1), "A production of       TheGrumpyGameDev", False, Hue.Black)
        buffer.WriteText((0, 3), "With ""help"" from his  malcontent and        neer-do-well twitch   channel viewers.", False, Hue.Black)
        buffer.WriteText((0, 14), "Special thanks to:", False, Hue.Black)
        buffer.WriteText((0, 15), "* Kordanor", False, Hue.Black)
        buffer.WriteText((0, 16), "* Zooperdan", False, Hue.Black)
        buffer.WriteText((0, 17), "* Vermux", False, Hue.Black)
        buffer.WriteText((0, 18), "* Lorc", False, Hue.Black)
        buffer.WriteText((0, 20), "Please see Credits.txtfor links to their    work!", False, Hue.Purple)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Green, Command.Blue, Command.Red
                Return UIState.TitleScreen
            Case Else
                Return UIState.AboutScreen
        End Select
    End Function
End Class
