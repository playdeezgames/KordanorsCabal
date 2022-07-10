Friend Class BlackMageModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const OffersButtonIndex = 1
    Const SellButtonIndex = 2
    Const GoodByeButtonIndex = 9
    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7


    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Big Black Mage Blessings To You!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(OffersButtonIndex).Title = "Offers"
        Buttons(SellButtonIndex).Title = "Sell"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case OffersButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.BlackMage
                Return UIState.ShoppeOffers
            Case SellButtonIndex
                ShoppeProcessor(Of Item).ShoppeType = Game.ShoppeType.BlackMage
                Return UIState.ShoppeSell
            Case PricesButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.BlackMage
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                ShoppeProcessor(Of (ItemType, Long)).ShoppeType = Game.ShoppeType.BlackMage
                Return UIState.ShoppeBuy
        End Select
        Return UIState.InPlay
    End Function
End Class
