Imports SPLORR.Data

Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnButtonIndex = 0
    Const MoveButtonIndex = 5
    Const InteractButtonIndex = 1

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
        buffer.WriteText((0, 0), "╲                    ╱", False, Hue.Black)
        buffer.WriteText((0, 1), " ╲                  ╱ ", False, Hue.Black)
        buffer.WriteText((0, 2), "  ╲                ╱  ", False, Hue.Black)
        buffer.WriteText((0, 15), "  ╱                ╲  ", False, Hue.Black)
        buffer.WriteText((0, 16), " ╱                  ╲ ", False, Hue.Black)
        buffer.WriteText((0, 17), "╱                    ╲", False, Hue.Black)
        buffer.FillCells((4, 3), (14, 1), Pattern.Horizontal1, False, Hue.Black)
        buffer.FillCells((4, 14), (14, 1), Pattern.Horizontal8, False, Hue.Black)
        buffer.FillCells((3, 4), (1, 10), Pattern.Vertical1, False, Hue.Black)
        buffer.FillCells((18, 4), (1, 10), Pattern.Vertical8, False, Hue.Black)
        buffer.PutCell((3, 3), Pattern.TopLeftCorner, False, Hue.Black)
        buffer.PutCell((18, 3), Pattern.TopRightCorner, False, Hue.Black)
        buffer.PutCell((3, 14), Pattern.BottomLeftCorner, False, Hue.Black)
        buffer.PutCell((18, 14), Pattern.BottomRightCorner, False, Hue.Black)
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
