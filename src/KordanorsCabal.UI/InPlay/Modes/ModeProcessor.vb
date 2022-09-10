Imports KordanorsCabal.Data

Public MustInherit Class ModeProcessor
    Protected Shared Sub ShowHeader(buffer As PatternBuffer, title As String)
        buffer.WriteText((0, 0), "                      ", True, Hue.Blue)
        buffer.WriteTextCentered(0, title, True, Hue.Blue)
    End Sub
    Protected Shared Sub ShowFacing(buffer As PatternBuffer, xy As (Integer, Integer), player As ICharacter)
        buffer.WriteText(xy, $"Facing: {player.Direction.Name}", False, Hue.Black)
    End Sub
    Protected Shared Sub ShowExits(buffer As PatternBuffer, xy As (Integer, Integer), player As ICharacter)
        Dim exits = String.Join(",", player.Location.RouteDirections.Select(Function(x) x.Abbreviation))
        buffer.WriteText(xy, $"Exits: {exits}", False, Hue.Black)
    End Sub

    Friend Shared Sub ResetButtonIndexStack()
        currentButtonStack.Clear()
        CurrentButtonIndex = 0
    End Sub

    Friend MustOverride Sub UpdateBuffer(player As ICharacter, buffer As PatternBuffer)
    Friend MustOverride Sub UpdateButtons(player As ICharacter)
    Friend MustOverride Function HandleButton(player As ICharacter, button As Button) As UIState
    Friend MustOverride Function HandleRed(player As ICharacter) As UIState
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
    Protected Sub ShowDungeon(buffer As PatternBuffer, player As ICharacter)
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
            buffer.WriteText((10, 9), routeType.Abbreviation, False, routeType.TextHue)
        End If

        If location.HasRoute(player.Direction.PreviousDirection) Then
            buffer.PutCell((0, 3), Pattern.DownwardDiagonal, False, Hue.Black)
            buffer.PutCell((1, 4), Pattern.DownwardDiagonal, False, Hue.Black)
            buffer.FillCells((1, 5), (1, 12), Pattern.Vertical8, False, Hue.Black)
            Dim routeType = location.Routes(player.Direction.PreviousDirection).RouteType
            buffer.WriteText((0, 9), routeType.Abbreviation.Substring(1, 1), False, routeType.TextHue)
        End If

        If location.HasRoute(player.Direction.NextDirection) Then
            buffer.PutCell((21, 3), Pattern.UpwardDiagonal, False, Hue.Black)
            buffer.PutCell((20, 4), Pattern.UpwardDiagonal, False, Hue.Black)
            buffer.FillCells((20, 5), (1, 12), Pattern.Vertical1, False, Hue.Black)
            Dim routeType = location.Routes(player.Direction.NextDirection).RouteType
            buffer.WriteText((21, 9), routeType.Abbreviation.Substring(0, 1), False, routeType.TextHue)
        End If

        If location.HasRoute(Direction.FromId(StaticWorldData.World, 5L)) Then
            buffer.PutCell((7, 0), Pattern.DownwardDiagonal, False, Hue.Black)
            buffer.FillCells((8, 0), (6, 1), Pattern.Horizontal8, False, Hue.Black)
            buffer.PutCell((14, 0), Pattern.UpwardDiagonal, False, Hue.Black)
        End If

        If location.HasRoute(Direction.FromId(StaticWorldData.World, 6L)) Then
            buffer.PutCell((7, 17), Pattern.UpwardDiagonal, False, Hue.Black)
            buffer.FillCells((8, 17), (6, 1), Pattern.Horizontal1, False, Hue.Black)
            buffer.PutCell((14, 17), Pattern.DownwardDiagonal, False, Hue.Black)
        End If

        If location.HasRoute(Direction.FromId(StaticWorldData.World, 8L)) Then
            ShowSprite(buffer, (5, 5), location.Routes(Direction.FromId(StaticWorldData.World, 8L)).RouteType.Sprite)
        End If

        If location.HasRoute(Direction.FromId(StaticWorldData.World, 7L)) Then
            ShowSprite(buffer, (5, 5), location.Routes(Direction.FromId(StaticWorldData.World, 7L)).RouteType.Sprite)
        End If

        For Each item In player.Location.Inventory.Items
            Dim itemType As OldItemType = item.ItemType
            If itemType.DisplayPattern.HasValue Then
                buffer.PutCell(itemType.DisplayXY.Value, itemType.DisplayPattern.Value, False, itemType.DisplayHue.Value)
            End If
        Next

        Dim enemy = player.Location.Enemies(player).FirstOrDefault
        If enemy IsNot Nothing Then
            ShowEnemy(buffer, (5, 5), enemy)
        End If

        buffer.WriteTextCentered(1, player.Direction.Abbreviation, False, Hue.Purple)
        If player.IsEncumbered Then
            buffer.WriteTextCentered(2, "Encumbered", False, Hue.Red)
        End If
    End Sub

    Private Sub ShowEnemy(buffer As PatternBuffer, spriteXY As (Integer, Integer), enemy As ICharacter)
        ShowSprite(buffer, spriteXY, enemy.CharacterType.Sprite)
    End Sub

    Private Shared Sub ShowSprite(buffer As PatternBuffer, spriteXY As (Integer, Integer), sprite As Sprite)
        For Each pixel In sprite.Pixels
            Dim xy = (pixel.Key.Item1 + spriteXY.Item1, pixel.Key.Item2 + spriteXY.Item2)
            Dim pattern = pixel.Value.Item1
            Dim inverted = pixel.Value.Item2
            buffer.PutCell(xy, pattern, inverted, sprite.Hue)
        Next
    End Sub
End Class
