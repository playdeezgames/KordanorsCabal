Friend Class BlacksmithModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const OffersButtonIndex = 1
    Const SellButtonIndex = 2
    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Watch out for the moon people! They'll come down from the moon and tie yer shoes together you know!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Hullo therrrre! What can I do for ye?", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(OffersButtonIndex).Title = "Offers"
        Buttons(SellButtonIndex).Title = "Sell"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case OffersButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.Blacksmith
                Return UIState.ShoppeOffers
            Case SellButtonIndex
                ShoppeProcessor(Of Item).ShoppeType = Game.ShoppeType.Blacksmith
                Return UIState.ShoppeSell
            Case PricesButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.Blacksmith
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                ShoppeProcessor(Of (ItemType, Long)).ShoppeType = Game.ShoppeType.Blacksmith
                Return UIState.ShoppeBuy
        End Select
        Return UIState.InPlay
    End Function
End Class
