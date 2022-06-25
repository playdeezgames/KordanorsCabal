Friend Class TurnModeProcessor
    Inherits ModeProcessor

    Const CancelButtonIndex = 6
    Const LeftButtonIndex = 0
    Const RightButtonIndex = 5
    Const AroundButtonIndex = 1

    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        Dim location = player.Location
        Select Case location.LocationType
            Case LocationType.Dungeon, LocationType.DungeonDeadEnd, LocationType.DungeonBoss
                ShowDungeon(buffer, player)
            Case Else
                ShowHeader(buffer, player.Location.Name)
                ShowFacing(buffer, (0, 1), player)
                ShowExits(buffer, (0, 2), player)

                buffer.WriteText((0, 4), $"Turn which way?", False, Hue.Purple)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(CancelButtonIndex).Title = "Cancel"
        Buttons(LeftButtonIndex).Title = "Left"
        Buttons(RightButtonIndex).Title = "Right"
        Buttons(AroundButtonIndex).Title = "Around"
    End Sub

    Friend Overrides Sub HandleButton(player As PlayerCharacter, button As Button)
        Select Case button.Index
            Case CancelButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case AroundButtonIndex
                PopButtonIndex()
                player.Direction = player.Direction.Opposite
                player.Mode = PlayerMode.Neutral
            Case LeftButtonIndex
                PopButtonIndex()
                player.Direction = player.Direction.PreviousDirection.Value
                player.Mode = PlayerMode.Neutral
            Case RightButtonIndex
                PopButtonIndex()
                player.Direction = player.Direction.NextDirection.Value
                player.Mode = PlayerMode.Neutral
        End Select
    End Sub
End Class
