Friend Class ConfirmAbandonGameProcessor
    Inherits MenuProcessor

    Const NoMenuItem = "No"
    Const YesMenuItem = "Yes"
    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                (NoMenuItem, Function() UIState.InPlay),
                (YesMenuItem, AddressOf AbandonGame)
            },
            14,
            UIState.ConfirmAbandonGame)
    End Sub

    Private Shared Function AbandonGame() As UIState
        ModeProcessor.ResetButtonIndexStack()
        Return UIState.TitleScreen
    End Function

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteText((0, 10), "Are you sure you want to abandon the game?", False, Hue.Red)
    End Sub

    Public Overrides Sub Initialize()

        currentItem = 0
    End Sub

    Public Overrides Function HandleRed() As UIState
        Return UIState.InPlay
    End Function
End Class
