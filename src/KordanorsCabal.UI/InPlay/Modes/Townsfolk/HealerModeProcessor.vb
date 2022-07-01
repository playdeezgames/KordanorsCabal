Friend Class HealerModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const HealButtonIndex = 1

    Const BuyPotionIndex = 5

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
            Case BuyPotionIndex
                buffer.WriteText((0, 1), $"I sell potions for {ItemType.Potion.PurchasePrice.Value} money.", False, Hue.Black)
                buffer.WriteText((0, 3), $"You have {player.Money} money.", False, Hue.Black)
                buffer.WriteText((0, 4), $"You have {player.GetItemTypeCount(ItemType.Potion)} potions.", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), defaultResponse, False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        If player.NeedsHealing Then
            Buttons(HealButtonIndex).Title = "Heal me!"
        End If
        Buttons(BuyPotionIndex).Title = "Buy Potion"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case HealButtonIndex
                player.Heal()
            Case BuyPotionIndex
                player.Buy(ItemType.Potion)
        End Select
        Return UIState.InPlay
    End Function
End Class
