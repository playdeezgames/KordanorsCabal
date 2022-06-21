Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnButtonIndex = 0
    Const MoveButtonIndex = 5

    Friend Overrides Sub UpdateBuffer(buffer As PatternBuffer)
        Dim player = World.PlayerCharacter
        ShowHeader(buffer, player.Location.Name)
        ShowFacing(buffer, (0, 1), player)
        ShowExits(buffer, (0, 2), player)
    End Sub
    Friend Overrides Sub UpdateButtons()
        Buttons(TurnButtonIndex).Title = "Turn..."
        Buttons(MoveButtonIndex).Title = "Move..."
    End Sub

    Friend Overrides Sub HandleButton(button As Button)
        Select Case button.Index
            Case TurnButtonIndex
                PushButtonIndex()
                World.PlayerCharacter.Mode = PlayerMode.Turn
        End Select
    End Sub
End Class
