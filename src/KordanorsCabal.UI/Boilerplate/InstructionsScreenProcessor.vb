Friend Class InstructionsScreenProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 0), "     Instructions     ", True, Hue.Blue)

        buffer.WriteLines((0, 2), New List(Of String) From
                          {
                            "The game is pretty",
                            "much menu driven. Use",
                            "arrow keys, space, ",
                            "enter, and rarely esc."
                          }, False, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Green, Command.Blue
                Return UIState.TitleScreen
            Case Else
                Return UIState.InstructionsScreen
        End Select
    End Function
End Class
