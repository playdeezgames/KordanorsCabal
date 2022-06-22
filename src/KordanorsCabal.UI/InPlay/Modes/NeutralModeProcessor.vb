Imports SPLORR.Data

Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnButtonIndex = 0
    Const MoveButtonIndex = 5

    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Name)
        ShowFacing(buffer, (0, 1), player)
        ShowExits(buffer, (0, 2), player)
    End Sub
    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(TurnButtonIndex).Title = "Turn..."
        Buttons(MoveButtonIndex).Title = "Move..."
    End Sub

    Friend Overrides Sub HandleButton(player As PlayerCharacter, button As Button)
        Select Case button.Index
            Case TurnButtonIndex
                PushButtonIndex()
                World.PlayerCharacter.Mode = PlayerMode.Turn
            Case MoveButtonIndex
                PushButtonIndex()
                World.PlayerCharacter.Mode = PlayerMode.Move
                CurrentButtonIndex = 0
            Case 9
                Store.Save("temp.db")
        End Select
    End Sub
End Class
