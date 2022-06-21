Friend Class InPlayProcessor
    Implements IProcessor

    Private _buttons As New List(Of Button) From
        {
            New Button(0, "Turn...", (0, 18), 11)
        }
    Private _currentButton As Integer = 0


    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 0), "                      ", True, Hue.Blue)
        Dim player = World.PlayerCharacter
        Dim location = player.Location
        buffer.WriteTextCentered(0, location.Name, True, Hue.Blue)
        buffer.WriteText((0, 1), $"Facing: {player.Direction.Name}", False, Hue.Black)
        Dim exits = String.Join(",", location.Routes.Select(Function(x) x.Key.Abbreviation))
        buffer.WriteText((0, 2), $"Exits: {exits}", False, Hue.Black)

        DrawButtons(buffer)
    End Sub

    Private Sub DrawButtons(buffer As PatternBuffer)
        For Each button In _buttons
            button.Draw(buffer, _currentButton)
        Next
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Green, Command.Blue
                Return UIState.InPlay
            Case Else
                Return UIState.InPlay
        End Select
    End Function
End Class
