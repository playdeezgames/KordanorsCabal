Friend Class AboutScreenProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 0), "About", False, Hue.Blue)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Green, Command.Blue
                Return UIState.TitleScreen
            Case Else
                Return UIState.AboutScreen
        End Select
    End Function
End Class
