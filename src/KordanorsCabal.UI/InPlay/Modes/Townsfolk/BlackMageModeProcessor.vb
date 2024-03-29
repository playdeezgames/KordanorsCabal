﻿Friend Class BlackMageModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const OffersButtonIndex = 1
    Const SellButtonIndex = 2
    Const RestoreButtonIndex = 5
    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As IPlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Movement.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Big Black Mage Blessings To You!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As IPlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(OffersButtonIndex).Title = "Offers"
        Buttons(SellButtonIndex).Title = "Sell"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
        If player.Mana.CurrentMana < player.Mana.MaximumMana Then
            Buttons(RestoreButtonIndex).Title = "Restore"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As IPlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case OffersButtonIndex
                World.FromWorldData(WorldData).PlayerCharacter.ShoppeType = ShoppeType.FromId(WorldData, 1)
                Return UIState.ShoppeOffers
            Case SellButtonIndex
                World.FromWorldData(WorldData).PlayerCharacter.ShoppeType = ShoppeType.FromId(WorldData, 1)
                Return UIState.ShoppeSell
            Case PricesButtonIndex
                World.FromWorldData(WorldData).PlayerCharacter.ShoppeType = ShoppeType.FromId(WorldData, 1)
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                World.FromWorldData(WorldData).PlayerCharacter.ShoppeType = ShoppeType.FromId(WorldData, 1)
                Return UIState.ShoppeBuy
            Case RestoreButtonIndex
                'TODO: push this into an event!
                player.EnqueueMessage(Nothing, $"{New FeatureType(WorldData, FeatureType6).Name} sparks up his {ItemType.FromId(WorldData, ItemType33).Name} and gives you a hit of {ItemType.FromId(WorldData, ItemType34).Name}.")
                player.Mana.CurrentMana = player.Mana.MaximumMana
                PushUIState(UIState.InPlay)
                Return UIState.Message
        End Select
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As IPlayerCharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
