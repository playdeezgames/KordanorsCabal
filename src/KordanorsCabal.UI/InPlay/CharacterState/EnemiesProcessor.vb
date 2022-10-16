Imports KordanorsCabal.Data

Friend Class EnemiesProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Enemies", True, Hue.Blue)
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)
        Dim enemies = player.Movement.Location.Factions.EnemiesOf(player)
        Dim row = 1
        For Each enemy In enemies
            buffer.WriteText((0, row), $"{enemy.CharacterType.Name}({enemy.Health.Current}/{enemy.Health.Maximum})", False, Hue.Black)
            row += 1
        Next
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Blue, Command.Green, Command.Red
                Return UIState.InPlay
            Case Else
                Return UIState.Enemies
        End Select
    End Function
End Class
