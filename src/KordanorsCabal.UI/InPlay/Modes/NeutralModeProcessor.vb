Imports SPLORR.Data

Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnButtonIndex = 0
    Const MoveButtonIndex = 5
    Const InteractButtonIndex = 1
    Const GroundButtonIndex = 2

    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        Dim location = player.Location
        Select Case location.LocationType
            Case LocationType.Dungeon
                ShowDungeon(buffer, player)
            Case Else
                ShowHeader(buffer, location.Name)
                ShowFacing(buffer, (0, 1), player)
                ShowExits(buffer, (0, 2), player)
                ShowFeatures(buffer, (0, 3), player)
        End Select
    End Sub

    Private Sub ShowDungeon(buffer As PatternBuffer, player As PlayerCharacter)
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
        End If

        If location.HasRoute(player.Direction.PreviousDirection.Value) Then
            buffer.PutCell((0, 3), Pattern.DownwardDiagonal, False, Hue.Black)
            buffer.PutCell((1, 4), Pattern.DownwardDiagonal, False, Hue.Black)
            buffer.FillCells((1, 5), (1, 12), Pattern.Vertical8, False, Hue.Black)
        End If

        If location.HasRoute(player.Direction.NextDirection.Value) Then
            buffer.PutCell((21, 3), Pattern.UpwardDiagonal, False, Hue.Black)
            buffer.PutCell((20, 4), Pattern.UpwardDiagonal, False, Hue.Black)
            buffer.FillCells((20, 5), (1, 12), Pattern.Vertical1, False, Hue.Black)
        End If
    End Sub

    Private Sub ShowFeatures(buffer As PatternBuffer, xy As (Integer, Integer), player As PlayerCharacter)
        Dim feature = player.Location.Feature
        If feature IsNot Nothing Then
            buffer.WriteText(xy, $"You see: {feature.Name}", False, Hue.Black)
        End If
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(TurnButtonIndex).Title = "Turn..."
        Buttons(MoveButtonIndex).Title = "Move..."
        If player.CanInteract Then
            Buttons(InteractButtonIndex).Title = "Interact..."
        End If
        If Not player.Location.Inventory.IsEmpty Then
            Buttons(GroundButtonIndex).Title = "Ground..."
        End If
    End Sub

    Friend Overrides Sub HandleButton(player As PlayerCharacter, button As Button)
        Select Case button.Index
            Case TurnButtonIndex
                PushButtonIndex(0)
                player.Mode = PlayerMode.Turn
            Case MoveButtonIndex
                PushButtonIndex(0)
                player.Mode = PlayerMode.Move
            Case InteractButtonIndex
                If player.CanInteract Then
                    PushButtonIndex(0)
                    player.Interact()
                End If
            Case 9
                Store.Save("temp.db")
        End Select
    End Sub
End Class
