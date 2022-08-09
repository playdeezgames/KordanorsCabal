Friend Class MapProcessor
    Implements IProcessor
    Private redrawBuffer As Boolean

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        If redrawBuffer Then
            redrawBuffer = False
            buffer.Fill(Pattern.Space, False, Hue.Blue)
            Dim player = World.PlayerCharacter
            Dim playerLocation = player.Location
            Dim level = playerLocation.DungeonLevel
            If level <> DungeonLevel.None Then
                Dim locations = Location.ByDungeonLevel(level).Where(Function(x) player.HasVisited(x))
                For Each location In locations
                    Dim inverted = (location = playerLocation)
                    Dim dungeonColumn = location.GetStatistic(LocationStatisticType.DungeonColumn).Value
                    Dim dungeonRow = location.GetStatistic(LocationStatisticType.DungeonRow).Value
                    Dim displayHue =
                        If(location.Enemies(player).Any, Hue.Pink,
                        If(location.HasStairs, Hue.Green,
                        If(location.Inventory.IsEmpty, Hue.Black, Hue.Purple)))
                    DrawLocation(buffer, (CInt(dungeonColumn * 2), CInt(dungeonRow * 2)), location, inverted, displayHue)
                Next
            End If
            buffer.WriteText((2, 22), "Stair", True, Hue.Green)
            buffer.WriteText((8, 22), "Enemy", True, Hue.Pink)
            buffer.WriteText((14, 22), "Items", True, Hue.Purple)
        End If
    End Sub

    Private Sub DrawLocation(buffer As PatternBuffer, xy As (Integer, Integer), location As Location, inverted As Boolean, displayHue As Hue)
        Dim northExit = location.HasRoute(Direction.North.ToDescriptor)
        Dim eastExit = location.HasRoute(Direction.East.ToDescriptor)
        Dim southExit = location.HasRoute(Direction.South.ToDescriptor)
        Dim westExit = location.HasRoute(Direction.West.ToDescriptor)
        'upper left
        buffer.PutCell(xy,
                       If(northExit AndAlso westExit, Pattern.ElbowUpLeft,
                       If(northExit, Pattern.Vertical5,
                       If(westExit, Pattern.Horizontal5, Pattern.ElbowDownRight))), inverted, displayHue)
        'upper right
        buffer.PutCell((xy.Item1 + 1, xy.Item2),
                       If(northExit AndAlso eastExit, Pattern.ElbowUpRight,
                       If(northExit, Pattern.Vertical5,
                       If(eastExit, Pattern.Horizontal5, Pattern.ElbowDownLeft))), inverted, displayHue)
        'lower left
        buffer.PutCell((xy.Item1, xy.Item2 + 1),
                       If(southExit AndAlso westExit, Pattern.ElbowDownLeft,
                       If(southExit, Pattern.Vertical5,
                       If(westExit, Pattern.Horizontal5, Pattern.ElbowUpRight))), inverted, displayHue)
        'lower right
        buffer.PutCell((xy.Item1 + 1, xy.Item2 + 1),
                       If(southExit AndAlso eastExit, Pattern.ElbowDownRight,
                       If(southExit, Pattern.Vertical5,
                       If(eastExit, Pattern.Horizontal5, Pattern.ElbowUpLeft))), inverted, displayHue)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
        redrawBuffer = True
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Return UIState.InPlay
    End Function
End Class
