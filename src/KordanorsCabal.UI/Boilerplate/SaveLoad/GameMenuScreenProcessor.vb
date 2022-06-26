Friend Class GameMenuScreenProcessor
    Inherits MenuProcessor

    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Go Back", Function() UIState.InPlay),
                ("Save Game", AddressOf SaveGame),
                ("Abandon Game", Function() UIState.ConfirmAbandonGame)
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

    Public Overrides Sub Initialize()
        currentItem = 0
    End Sub
End Class
