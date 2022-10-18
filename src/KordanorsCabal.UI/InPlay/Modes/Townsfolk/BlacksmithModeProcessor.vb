Friend Class BlacksmithModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const OffersButtonIndex = 1
    Const SellButtonIndex = 2
    Const RepairButtonIndex = 3
    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As IPlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Movement.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Watch out for the moon people! They'll come down from the moon and tie yer shoes together you know!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Hullo therrrre! What can I do for ye?", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As IPlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(OffersButtonIndex).Title = "Offers"
        Buttons(SellButtonIndex).Title = "Sell"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
        If player.Repair.HasItemsToRepair(ShoppeType.FromId(StaticWorldData.World, 2)) Then
            Buttons(RepairButtonIndex).Title = "Repair"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As IPlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case OffersButtonIndex
                ShoppeProcessor(Of String).ShoppeType = ShoppeType.FromId(StaticWorldData.World, 2)
                Return UIState.ShoppeOffers
            Case SellButtonIndex
                ShoppeProcessor(Of Item).ShoppeType = ShoppeType.FromId(StaticWorldData.World, 2)
                Return UIState.ShoppeSell
            Case PricesButtonIndex
                ShoppeProcessor(Of String).ShoppeType = ShoppeType.FromId(StaticWorldData.World, 2)
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                ShoppeProcessor(Of (IItemType, Long)).ShoppeType = ShoppeType.FromId(StaticWorldData.World, 2)
                Return UIState.ShoppeBuy
            Case RepairButtonIndex
                If player.Repair.HasItemsToRepair(ShoppeType.FromId(StaticWorldData.World, 2)) Then
                    ShoppeProcessor(Of Item).ShoppeType = ShoppeType.FromId(StaticWorldData.World, 2)
                    Return UIState.ShoppeRepair
                End If
        End Select
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As IPlayerCharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
