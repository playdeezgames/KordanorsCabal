Friend Class TurnModeProcessor
    Inherits ModeProcessor

    Const CancelButtonIndex = 6
    Const LeftButtonIndex = 0
    Const RightButtonIndex = 5
    Const AroundButtonIndex = 1

    Friend Overrides Sub UpdateBuffer(buffer As PatternBuffer)
        buffer.WriteText((0, 0), "                      ", True, Hue.Blue)
        Dim player = World.PlayerCharacter
        Dim location = player.Location
        buffer.WriteTextCentered(0, location.Name, True, Hue.Blue)
        buffer.WriteText((0, 1), $"Facing: {player.Direction.Name}", False, Hue.Black)
        buffer.WriteText((0, 3), $"Turn which way?", False, Hue.Purple)
    End Sub

    Friend Overrides Sub UpdateButtons(buttons As IReadOnlyList(Of Button))
        buttons(CancelButtonIndex).Title = "Cancel"
        buttons(LeftButtonIndex).Title = "Left"
        buttons(RightButtonIndex).Title = "Right"
        buttons(AroundButtonIndex).Title = "Around"
    End Sub

    Friend Overrides Sub HandleButton(button As Button)
        Select Case button.Index
            Case CancelButtonIndex
                World.PlayerCharacter.Mode = PlayerMode.Neutral
        End Select
    End Sub
End Class
