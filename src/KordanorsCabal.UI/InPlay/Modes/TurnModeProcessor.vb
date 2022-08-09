Friend Class TurnModeProcessor
    Inherits ModeProcessor

    Const CancelButtonIndex = 6
    Const LeftButtonIndex = 0
    Const RightButtonIndex = 5
    Const AroundButtonIndex = 1

    Friend Overrides Sub UpdateBuffer(player As Character, buffer As PatternBuffer)
        Dim location = player.Location
        If location.LocationType.IsDungeon Then
            ShowDungeon(buffer, player)
        Else
            ShowHeader(buffer, player.Location.Name)
            ShowFacing(buffer, (0, 1), player)
            ShowExits(buffer, (0, 2), player)

            buffer.WriteText((0, 4), $"Turn which way?", False, Hue.Purple)
        End If
    End Sub

    Friend Overrides Sub UpdateButtons(player As Character)
        Buttons(CancelButtonIndex).Title = "Cancel"
        Buttons(LeftButtonIndex).Title = "Left"
        Buttons(RightButtonIndex).Title = "Right"
        Buttons(AroundButtonIndex).Title = "Around"
    End Sub

    Friend Overrides Function HandleButton(player As Character, button As Button) As UIState
        Select Case button.Index
            Case CancelButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case AroundButtonIndex
                PopButtonIndex()
                player.Direction = player.Direction.ToDescriptor.Opposite
                player.Mode = PlayerMode.Neutral
            Case LeftButtonIndex
                PopButtonIndex()
                player.Direction = player.Direction.ToDescriptor.PreviousDirection.Value
                player.Mode = PlayerMode.Neutral
            Case RightButtonIndex
                PopButtonIndex()
                player.Direction = player.Direction.ToDescriptor.NextDirection.Value
                player.Mode = PlayerMode.Neutral
        End Select
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As Character) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
