Friend Class InstructionsScreenProcessor
    Inherits BaseProcessor

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
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

    Public Overrides Sub Initialize()
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
        Select Case command
            Case Command.Green, Command.Blue, Command.Red
                Return UIState.TitleScreen
            Case Else
                Return UIState.InstructionsScreen
        End Select
    End Function
End Class
