﻿Friend Class TurnModeProcessor
    Inherits ModeProcessor

    Const CancelButtonIndex = 6
    Const LeftButtonIndex = 0
    Const RightButtonIndex = 5
    Const AroundButtonIndex = 1

    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
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

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(CancelButtonIndex).Title = "Cancel"
        Buttons(LeftButtonIndex).Title = "Left"
        Buttons(RightButtonIndex).Title = "Right"
        Buttons(AroundButtonIndex).Title = "Around"
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
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
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As PlayerCharacter) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
