Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnButtonIndex = 0

    Friend Overrides Sub UpdateBuffer(buffer As PatternBuffer)
        buffer.WriteText((0, 0), "                      ", True, Hue.Blue)
        Dim player = World.PlayerCharacter
        Dim location = player.Location
        buffer.WriteTextCentered(0, location.Name, True, Hue.Blue)
        buffer.WriteText((0, 1), $"Facing: {player.Direction.Name}", False, Hue.Black)
        Dim exits = String.Join(",", location.Routes.Select(Function(x) x.Key.Abbreviation))
        buffer.WriteText((0, 2), $"Exits: {exits}", False, Hue.Black)
    End Sub

    Friend Overrides Sub UpdateButtons(buttons As IReadOnlyList(Of Button))
        For Each button In buttons
            button.Title = ""
        Next
        buttons(TurnButtonIndex).Title = "Turn..."
    End Sub

    Friend Overrides Sub HandleButton(button As Button)
        Select Case button.Index
            Case TurnButtonIndex
                'TODO: go to the "turn" mode
        End Select
    End Sub
End Class
