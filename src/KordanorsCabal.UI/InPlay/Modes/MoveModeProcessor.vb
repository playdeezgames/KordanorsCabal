Friend Class MoveModeProcessor
    Inherits ModeProcessor

    Const CancelButtonIndex = 4
    Const ForwardButtonIndex = 0
    Const BackwardButtonIndex = 5
    Const LeftButtonIndex = 1
    Const RightButtonIndex = 6
    Const UpButtonIndex = 2
    Const InButtonIndex = 3
    Const DownButtonIndex = 7
    Const OutButtonIndex = 8

    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Name)
        ShowFacing(buffer, (0, 1), player)
        ShowExits(buffer, (0, 2), player)

        buffer.WriteText((0, 4), $"mOVE which way?", False, Hue.Purple)
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(CancelButtonIndex).Title = "Cancel"
        If player.CanMoveLeft Then
            Buttons(LeftButtonIndex).Title = "Left"
        End If
        If player.CanMoveRight Then
            Buttons(RightButtonIndex).Title = "Right"
        End If
        If player.CanMoveForward Then
            Buttons(ForwardButtonIndex).Title = "Forward"
        End If
        If player.CanMoveBackward Then
            Buttons(BackwardButtonIndex).Title = "Backward"
        End If
        If player.CanMove(Direction.Up) Then
            Buttons(UpButtonIndex).Title = "Up"
        End If
        If player.CanMove(Direction.Down) Then
            Buttons(DownButtonIndex).Title = "Down"
        End If
        If player.CanMove(Direction.Inward) Then
            Buttons(InButtonIndex).Title = "In"
        End If
        If player.CanMove(Direction.Outward) Then
            Buttons(OutButtonIndex).Title = "Out"
        End If
    End Sub

    Friend Overrides Sub HandleButton(player As PlayerCharacter, button As Button)
        Select Case button.Index
            Case CancelButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case DownButtonIndex
                HandleMove(player, Direction.Down)
            Case UpButtonIndex
                HandleMove(player, Direction.Up)
            Case InButtonIndex
                HandleMove(player, Direction.Inward)
            Case OutButtonIndex
                HandleMove(player, Direction.Outward)
            Case ForwardButtonIndex
                HandleMove(player, player.Direction)
            Case BackwardButtonIndex
                HandleMove(player, player.Direction.Opposite)
            Case LeftButtonIndex
                HandleMove(player, player.Direction.PreviousDirection.Value)
            Case RightButtonIndex
                HandleMove(player, player.Direction.NextDirection.Value)
        End Select
    End Sub

    Private Sub HandleMove(player As PlayerCharacter, direction As Direction)
        If player.CanMove(direction) Then
            PopButtonIndex()
            player.Move(direction)
            player.Mode = PlayerMode.Neutral
        End If
    End Sub
End Class
