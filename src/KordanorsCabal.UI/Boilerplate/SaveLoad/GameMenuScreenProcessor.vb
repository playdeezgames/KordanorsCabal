Friend Class GameMenuScreenProcessor
    Inherits MenuProcessor

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Go Back", Function() UIState.InPlay),
                ("Save Game", AddressOf SaveGame)
            },
            5,
            UIState.GameMenuScreen)
    End Sub

    Private Shared Function SaveGame() As UIState
        Return UIState.SaveGameScreen
    End Function

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, "Game Menu", False, Hue.Blue)
    End Sub
End Class
