Friend Class TurnModeProcessor
    Inherits ModeProcessor

    Const CancelButtonIndex = 6
    Const LeftButtonIndex = 0
    Const RightButtonIndex = 5
    Const AroundButtonIndex = 1

    Friend Overrides Sub UpdateBuffer(player As ICharacter, buffer As PatternBuffer)
        Dim location = player.Movement.Location
        If location.LocationType.IsDungeon Then
            ShowDungeon(buffer, player)
        Else
            ShowHeader(buffer, player.Movement.Location.LocationType.Name)
            ShowFacing(buffer, (0, 1), player)
            ShowExits(buffer, (0, 2), player)

            buffer.WriteText((0, 4), $"Turn which way?", False, Hue.Purple)
        End If
    End Sub

    Friend Overrides Sub UpdateButtons(player As ICharacter)
        Buttons(CancelButtonIndex).Title = "Cancel"
        Buttons(LeftButtonIndex).Title = "Left"
        Buttons(RightButtonIndex).Title = "Right"
        Buttons(AroundButtonIndex).Title = "Around"
    End Sub

    Friend Overrides Function HandleButton(player As ICharacter, button As Button) As UIState
        Select Case button.Index
            Case CancelButtonIndex
                PopButtonIndex()
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case AroundButtonIndex
                PopButtonIndex()
                player.Movement.Direction = player.Movement.Direction.Opposite
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case LeftButtonIndex
                PopButtonIndex()
                player.Movement.Direction = player.Movement.Direction.PreviousDirection
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case RightButtonIndex
                PopButtonIndex()
                player.Movement.Direction = player.Movement.Direction.NextDirection
                player.Mode = Game.Constants.PlayerModes.Neutral
        End Select
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As ICharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
