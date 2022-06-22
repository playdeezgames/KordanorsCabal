Friend Class InPlayProcessor
    Implements IProcessor

    Private ReadOnly _modeProcessors As IReadOnlyDictionary(Of PlayerMode, ModeProcessor) =
        New Dictionary(Of PlayerMode, ModeProcessor) From
        {
            {PlayerMode.Move, New MoveModeProcessor},
            {PlayerMode.Neutral, New NeutralModeProcessor},
            {PlayerMode.Turn, New TurnModeProcessor}
        }


    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        Dim modeProcessor = _modeProcessors(World.PlayerCharacter.Mode)
        Dim player = World.PlayerCharacter
        modeProcessor.UpdateBuffer(player, buffer)
        For Each button In ModeProcessor.Buttons
            button.Clear()
        Next
        modeProcessor.UpdateButtons(player)
        DrawButtons(buffer)
    End Sub

    Private Sub DrawButtons(buffer As PatternBuffer)
        For Each button In ModeProcessor.Buttons
            button.Draw(buffer, ModeProcessor.CurrentButtonIndex)
        Next
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Green, Command.Blue
                _modeProcessors(World.PlayerCharacter.Mode).HandleButton(World.PlayerCharacter, ModeProcessor.Buttons(ModeProcessor.CurrentButtonIndex))
            Case Command.Down
                ModeProcessor.CurrentButtonIndex = (ModeProcessor.CurrentButtonIndex + 1) Mod ModeProcessor.Buttons.Count
            Case Command.Up
                ModeProcessor.CurrentButtonIndex = (ModeProcessor.CurrentButtonIndex + ModeProcessor.Buttons.Count - 1) Mod ModeProcessor.Buttons.Count
            Case Command.Left, Command.Right
                ModeProcessor.CurrentButtonIndex = (ModeProcessor.CurrentButtonIndex + ModeProcessor.Buttons.Count \ 2) Mod ModeProcessor.Buttons.Count
        End Select
        Return UIState.InPlay
    End Function
End Class
