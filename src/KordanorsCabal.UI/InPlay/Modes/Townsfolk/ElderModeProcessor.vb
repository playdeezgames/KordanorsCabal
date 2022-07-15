﻿Friend Class ElderModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const EncourageButtonIndex = 1
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case EncourageButtonIndex
                buffer.WriteText((0, 1), "If you find yerself disheartened, I'm always here to help!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Hello, my friend! Staya while, and listen!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(EncourageButtonIndex).Title = "Pep talk"
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case EncourageButtonIndex
                Return HandleEncouragement(player)
        End Select
        Return UIState.InPlay
    End Function

    Private Function HandleEncouragement(player As PlayerCharacter) As UIState
        player.EnqueueMessage("Sometimes in our life we all have pain. We all have sorry. But, if we are wise, we know that there's always tomorrow!")
        If player.CurrentMP < 1 Then
            player.CurrentMP = 1
        End If
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function
End Class
