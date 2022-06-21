Friend Class InPlayProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 0), "                      ", True, Hue.Blue)
        Dim player = World.PlayerCharacter
        Dim location = player.Location
        buffer.WriteTextCentered(0, location.Name, True, Hue.Blue)
        buffer.WriteText((0, 1), $"Facing: {player.Direction.Name}", False, Hue.Black)
        Dim exits = String.Join(",", location.Routes.Select(Function(x) x.Key.Abbreviation))
        buffer.WriteText((0, 2), $"Exits: {exits}", False, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Return UIState.InPlay
    End Function
End Class
