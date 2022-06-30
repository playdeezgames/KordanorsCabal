Friend Class HealerModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const HealButtonIndex = 1
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Const defaultResponse As String = "What ails you, my friend?"
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case HealButtonIndex
                If player.NeedsHealing Then
                    buffer.WriteText((0, 1), "Any time yer ready!", False, Hue.Black)
                Else
                    buffer.WriteText((0, 1), defaultResponse, False, Hue.Black)
                End If
            Case Else
                buffer.WriteText((0, 1), defaultResponse, False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        If player.NeedsHealing Then
            Buttons(HealButtonIndex).Title = "Heal me!"
        End If
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case HealButtonIndex
                player.SetStatistic(CharacterStatisticType.Wounds, 0)
        End Select
        Return UIState.InPlay
    End Function
End Class
