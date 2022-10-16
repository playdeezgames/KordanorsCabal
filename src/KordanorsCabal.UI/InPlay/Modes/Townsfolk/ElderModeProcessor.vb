Friend Class ElderModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const EncourageButtonIndex = 1
    Const MainQuestButtonIndex = 2
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As IPlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Movement.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case EncourageButtonIndex
                buffer.WriteText((0, 1), "If you find yerself disheartened, I'm always here to help!", False, Hue.Black)
            Case MainQuestButtonIndex
                buffer.WriteText((0, 1), "Deep within the cata- combs beneath the ab- andoned church there  is a demon named Kord-anor! Slay this foul  fiend and bring to me his horns, so that    there can be a lastingpeace!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Hello, my friend! Staya while, and listen!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As IPlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(EncourageButtonIndex).Title = "Pep talk"
        Buttons(MainQuestButtonIndex).Title = "The Cabal"
    End Sub

    Friend Overrides Function HandleButton(player As IPlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case EncourageButtonIndex
                Return HandleEncouragement(player)
        End Select
        Return UIState.InPlay
    End Function

    Private Function HandleEncouragement(player As ICharacter) As UIState
        player.EnqueueMessage(Nothing, "Sometimes in our life we all have pain. We all have sorrow. But, if we are wise, we know that there's always tomorrow!")
        If player.MentalCombat.CurrentMP < 1 Then
            player.MentalCombat.CurrentMP = 1
        End If
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function

    Friend Overrides Function HandleRed(player As IPlayerCharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
