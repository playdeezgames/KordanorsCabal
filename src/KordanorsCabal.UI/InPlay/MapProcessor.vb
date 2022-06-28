Friend Class MapProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteTextCentered(buffer.Rows - 1, "Esc", False, Hue.Black)
        Dim player = World.PlayerCharacter
        Dim playerLocation = player.Location
        Dim level = playerLocation.GetStatistic(LocationStatisticType.DungeonLevel)
        If level.HasValue Then
            Dim locations = Location.ByStatisticValue(LocationStatisticType.DungeonLevel, level.Value).Where(Function(x) player.HasVisited(x))
            For Each location In locations
                Dim inverted = (location = playerLocation)
                Dim dungeonColumn = location.GetStatistic(LocationStatisticType.DungeonColumn).Value
                Dim dungeonRow = location.GetStatistic(LocationStatisticType.DungeonRow).Value
                DrawLocation(buffer, (CInt(dungeonColumn * 2), CInt(dungeonRow * 2)), location, inverted)
            Next
        End If
    End Sub

    Private Sub DrawLocation(buffer As PatternBuffer, xy As (Integer, Integer), location As Location, inverted As Boolean)
        Dim northExit = location.HasRoute(Direction.North)
        Dim eastExit = location.HasRoute(Direction.East)
        Dim southExit = location.HasRoute(Direction.South)
        Dim westExit = location.HasRoute(Direction.West)
        'upper left
        buffer.PutCell(xy,
                       If(northExit AndAlso westExit, Pattern.ElbowUpLeft,
                       If(northExit, Pattern.Vertical5,
                       If(westExit, Pattern.Horizontal5, Pattern.ElbowDownRight))), inverted, Hue.Black)
        'upper right
        buffer.PutCell((xy.Item1 + 1, xy.Item2),
                       If(northExit AndAlso eastExit, Pattern.ElbowUpRight,
                       If(northExit, Pattern.Vertical5,
                       If(eastExit, Pattern.Horizontal5, Pattern.ElbowDownLeft))), inverted, Hue.Black)
        'lower left
        buffer.PutCell((xy.Item1, xy.Item2 + 1),
                       If(southExit AndAlso westExit, Pattern.ElbowDownLeft,
                       If(southExit, Pattern.Vertical5,
                       If(westExit, Pattern.Horizontal5, Pattern.ElbowUpRight))), inverted, Hue.Black)
        'lower right
        buffer.PutCell((xy.Item1 + 1, xy.Item2 + 1),
                       If(southExit AndAlso eastExit, Pattern.ElbowDownRight,
                       If(southExit, Pattern.Vertical5,
                       If(eastExit, Pattern.Horizontal5, Pattern.ElbowUpLeft))), inverted, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Red
                Return UIState.InPlay
            Case Else
                Return UIState.Map
        End Select
    End Function
End Class
