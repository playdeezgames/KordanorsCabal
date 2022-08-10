﻿Friend Class MoveModeProcessor
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

    Friend Overrides Sub UpdateBuffer(player As Character, buffer As PatternBuffer)
        Dim location = player.Location
        If location.LocationType.ToNew.IsDungeon Then
            ShowDungeon(buffer, player)
        Else
            ShowHeader(buffer, player.Location.Name)
            ShowFacing(buffer, (0, 1), player)
            ShowExits(buffer, (0, 2), player)

            buffer.WriteText((0, 4), $"Move which way?", False, Hue.Purple)
        End If
    End Sub

    Friend Overrides Sub UpdateButtons(player As Character)
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
        If player.CanMove(Direction.FromName("Up")) Then
            Buttons(UpButtonIndex).Title = "Up"
        End If
        If player.CanMove(Direction.FromName("Down")) Then
            Buttons(DownButtonIndex).Title = "Down"
        End If
        If player.CanMove(Direction.FromName(Inward)) Then
            Buttons(InButtonIndex).Title = "In"
        End If
        If player.CanMove(Direction.FromName(Outward)) Then
            Buttons(OutButtonIndex).Title = "Out"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As Character, button As Button) As UIState
        Select Case button.Index
            Case CancelButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case DownButtonIndex
                Return HandleMove(player, Direction.FromName("Down"))
            Case UpButtonIndex
                Return HandleMove(player, Direction.FromName("Up"))
            Case InButtonIndex
                Return HandleMove(player, Direction.FromName(Inward))
            Case OutButtonIndex
                Return HandleMove(player, Direction.FromName(Outward))
            Case ForwardButtonIndex
                Return HandleMove(player, player.Direction)
            Case BackwardButtonIndex
                Return HandleMove(player, player.Direction.Opposite)
            Case LeftButtonIndex
                Return HandleMove(player, player.Direction.PreviousDirection)
            Case RightButtonIndex
                Return HandleMove(player, player.Direction.NextDirection)
        End Select
        Return UIState.InPlay
    End Function

    Private Function HandleMove(player As Character, direction As Direction) As UIState
        If player.CanMove(direction) Then
            PopButtonIndex()
            player.Mode = PlayerMode.Neutral
            If player.Move(direction) Then
                player.EnqueueMessage("You take damage from starvation!")
                If player.IsDead Then
                    Return UIState.Dead
                End If
                PushUIState(UIState.InPlay)
                Return UIState.Message
            End If
        End If
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As Character) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
