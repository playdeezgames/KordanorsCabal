Public MustInherit Class ModeProcessor
    Protected Shared Sub ShowHeader(buffer As PatternBuffer, title As String)
        buffer.WriteText((0, 0), "                      ", True, Hue.Blue)
        buffer.WriteTextCentered(0, title, True, Hue.Blue)
    End Sub
    Protected Shared Sub ShowFacing(buffer As PatternBuffer, xy As (Integer, Integer), player As PlayerCharacter)
        buffer.WriteText(xy, $"Facing: {player.Direction.Name}", False, Hue.Black)
    End Sub
    Protected Shared Sub ShowExits(buffer As PatternBuffer, xy As (Integer, Integer), player As PlayerCharacter)
        Dim exits = String.Join(",", player.Location.Routes.Select(Function(x) x.Key.Abbreviation))
        buffer.WriteText(xy, $"Exits: {exits}", False, Hue.Black)
    End Sub
    Friend MustOverride Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
    Friend MustOverride Sub UpdateButtons(player As PlayerCharacter)
    Friend MustOverride Function HandleButton(player As PlayerCharacter, button As Button) As UIState
    Friend Shared Buttons As IReadOnlyList(Of Button) =
        New List(Of Button) From
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
    Private Shared currentButtonStack As New Stack(Of Integer)
    Friend Shared Property CurrentButtonIndex As Integer = 0
    Friend Shared Sub PushButtonIndex(newButtonIndex As Integer)
        currentButtonStack.Push(CurrentButtonIndex)
        CurrentButtonIndex = newButtonIndex
    End Sub
    Friend Shared Sub PopButtonIndex()
        If currentButtonStack.Any() Then
            CurrentButtonIndex = currentButtonStack.Pop()
            Return
        End If
        CurrentButtonIndex = 0
    End Sub
    Protected Sub ShowDungeon(buffer As PatternBuffer, player As PlayerCharacter)
        buffer.WriteText((0, 0), " ╲                  ╱", False, Hue.Black)
        buffer.WriteText((0, 1), "  ╲                ╱ ", False, Hue.Black)
        buffer.WriteText((0, 2), "   ╲              ╱  ", False, Hue.Black)
        buffer.WriteText((0, 15), "   ╱              ╲  ", False, Hue.Black)
        buffer.WriteText((0, 16), "  ╱                ╲ ", False, Hue.Black)
        buffer.WriteText((0, 17), " ╱                  ╲", False, Hue.Black)
        buffer.FillCells((5, 3), (12, 1), Pattern.Horizontal1, False, Hue.Black)
        buffer.FillCells((5, 14), (12, 1), Pattern.Horizontal8, False, Hue.Black)
        buffer.FillCells((4, 4), (1, 10), Pattern.Vertical1, False, Hue.Black)
        buffer.FillCells((17, 4), (1, 10), Pattern.Vertical8, False, Hue.Black)
        buffer.PutCell((4, 3), Pattern.TopLeftCorner, False, Hue.Black)
        buffer.PutCell((17, 3), Pattern.TopRightCorner, False, Hue.Black)
        buffer.PutCell((4, 14), Pattern.BottomLeftCorner, False, Hue.Black)
        buffer.PutCell((17, 14), Pattern.BottomRightCorner, False, Hue.Black)

        Dim location = player.Location
        If location.HasRoute(player.Direction) Then
            buffer.FillCells((9, 6), (4, 1), Pattern.Horizontal1, False, Hue.Black)
            buffer.FillCells((8, 6), (1, 8), Pattern.Vertical8, False, Hue.Black)
            buffer.FillCells((13, 6), (1, 8), Pattern.Vertical1, False, Hue.Black)
            buffer.PutCell((13, 14), Pattern.BottomLeftCorner, False, Hue.Black)
            buffer.PutCell((8, 14), Pattern.BottomRightCorner, False, Hue.Black)
            Dim routeType = location.Routes(player.Direction).RouteType
            buffer.WriteText((10, 9), routeType.Abbreviation, False, Hue.Blue)
        End If

        If location.HasRoute(player.Direction.PreviousDirection.Value) Then
            buffer.PutCell((0, 3), Pattern.DownwardDiagonal, False, Hue.Black)
            buffer.PutCell((1, 4), Pattern.DownwardDiagonal, False, Hue.Black)
            buffer.FillCells((1, 5), (1, 12), Pattern.Vertical8, False, Hue.Black)
            Dim routeType = location.Routes(player.Direction.PreviousDirection.Value).RouteType
            buffer.WriteText((0, 9), routeType.Abbreviation.Substring(1, 1), False, Hue.Blue)
        End If

        If location.HasRoute(player.Direction.NextDirection.Value) Then
            buffer.PutCell((21, 3), Pattern.UpwardDiagonal, False, Hue.Black)
            buffer.PutCell((20, 4), Pattern.UpwardDiagonal, False, Hue.Black)
            buffer.FillCells((20, 5), (1, 12), Pattern.Vertical1, False, Hue.Black)
            Dim routeType = location.Routes(player.Direction.NextDirection.Value).RouteType
            buffer.WriteText((21, 9), routeType.Abbreviation.Substring(0, 1), False, Hue.Blue)
        End If
    End Sub
End Class
