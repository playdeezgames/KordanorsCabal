Imports KordanorsCabal.Data

Friend Class EnemiesProcessor
    Inherits BaseProcessor

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Enemies", True, Hue.Blue)
        Dim player = Game.World.PlayerCharacter(worldData)
        Dim enemies = player.Movement.Location.Factions.EnemiesOf(player)
        Dim row = 1
        For Each enemy In enemies
            buffer.WriteText((0, row), $"{enemy.CharacterType.Name}({enemy.Health.Current}/{enemy.Health.Maximum})", False, Hue.Black)
            row += 1
        Next
    End Sub

    Public Overrides Sub Initialize()
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
        Select Case command
            Case Command.Blue, Command.Green, Command.Red
                Return UIState.InPlay
            Case Else
                Return UIState.Enemies
        End Select
    End Function
End Class
