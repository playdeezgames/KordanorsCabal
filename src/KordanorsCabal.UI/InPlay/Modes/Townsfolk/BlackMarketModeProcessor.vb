Friend Class BlackMarketModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9
    Const GambleButtonIndex = 5
    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7

    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Off with ye then!", False, Hue.Black)
                Return
            Case GambleButtonIndex
                If player.CanGamble Then
                    buffer.WriteText((0, 1), "Play two-up for 5 money! Try yer luck! Flip two coins, and if both are head, you win 15! Otherwise, I take yer 5!", False, Hue.Black)
                    buffer.WriteText((0, 8), $"You have {player.Money} money.", False, Hue.Black)
                    Return
                End If
        End Select
        buffer.WriteText((0, 1), "'Allo! 'Allo!", False, Hue.Black)
        buffer.WriteText((0, 2), "Would you like to buy a lovely pair of trousers?", False, Hue.Black)
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        If player.CanGamble Then
            Buttons(GambleButtonIndex).Title = "Gamble"
        End If
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case GambleButtonIndex
                If player.CanGamble Then
                    player.Gamble()
                    PushUIState(UIState.InPlay)
                    Return UIState.Message
                End If
            Case PricesButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.BlackMarket
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                ShoppeProcessor(Of (ItemType, Long)).ShoppeType = Game.ShoppeType.BlackMarket
                Return UIState.ShoppeBuy
        End Select
        Return UIState.InPlay
    End Function
End Class
