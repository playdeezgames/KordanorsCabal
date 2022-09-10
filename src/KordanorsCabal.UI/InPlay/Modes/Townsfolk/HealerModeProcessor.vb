Friend Class HealerModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const HealButtonIndex = 1

    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7

    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As ICharacter, buffer As PatternBuffer)
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

    Friend Overrides Sub UpdateButtons(player As ICharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        If player.NeedsHealing Then
            Buttons(HealButtonIndex).Title = "Heal me!"
        End If
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
    End Sub

    Friend Overrides Function HandleButton(player As ICharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case HealButtonIndex
                player.Heal()
            Case PricesButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.Healer
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                ShoppeProcessor(Of (OldItemType, Long)).ShoppeType = Game.ShoppeType.Healer
                Return UIState.ShoppeBuy
        End Select
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As ICharacter) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
