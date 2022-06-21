Friend Class InPlayProcessor
    Implements IProcessor

    Private _buttons As New List(Of Button) From
        {
            New Button(0, "0", (0, 18), 11),
            New Button(1, "1", (0, 19), 11),
            New Button(2, "2", (0, 20), 11),
            New Button(3, "3", (0, 21), 11),
            New Button(4, "4", (0, 22), 11),
            New Button(5, "5", (11, 18), 11),
            New Button(6, "6", (11, 19), 11),
            New Button(7, "7", (11, 20), 11),
            New Button(8, "8", (11, 21), 11),
            New Button(9, "9", (11, 22), 11)
        }
    Private _currentButton As Integer = 0
    Private _modeProcessors As IReadOnlyDictionary(Of PlayerMode, ModeProcessor) =
        New Dictionary(Of PlayerMode, ModeProcessor) From
        {
            {PlayerMode.Neutral, New NeutralModeProcessor},
            {PlayerMode.Turn, New TurnModeProcessor}
        }


    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        Dim modeProcessor = _modeProcessors(World.PlayerCharacter.Mode)
        modeProcessor.UpdateBuffer(buffer)
        For Each button In _buttons
            button.Clear()
        Next
        modeProcessor.UpdateButtons(_buttons)
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
                _modeProcessors(World.PlayerCharacter.Mode).HandleButton(_buttons(_currentButton))
            Case Command.Down
                _currentButton = (_currentButton + 1) Mod _buttons.Count
            Case Command.Up
                _currentButton = (_currentButton + _buttons.Count - 1) Mod _buttons.Count
            Case Command.Left, Command.Right
                _currentButton = (_currentButton + _buttons.Count \ 2) Mod _buttons.Count
        End Select
        Return UIState.InPlay
    End Function
End Class
