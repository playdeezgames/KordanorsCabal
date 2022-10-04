﻿Imports KordanorsCabal.Data

Friend Class BlackMageModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const OffersButtonIndex = 1
    Const SellButtonIndex = 2
    Const RestoreButtonIndex = 5
    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As ICharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Big Black Mage Blessings To You!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As ICharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(OffersButtonIndex).Title = "Offers"
        Buttons(SellButtonIndex).Title = "Sell"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
        If player.CurrentMana < player.MaximumMana Then
            Buttons(RestoreButtonIndex).Title = "Restore"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As ICharacter, button As Button) As UIState
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
                ShoppeProcessor(Of (OldItemType, Long)).ShoppeType = Game.ShoppeType.BlackMage
                Return UIState.ShoppeBuy
            Case RestoreButtonIndex
                player.EnqueueMessage($"{New FeatureType(StaticWorldData.World, 6L).Name} sparks up his {OldItemType.Bong.ToNew(StaticWorldData.World).Name} and gives you a hit of {OldItemType.Herb.ToNew(StaticWorldData.World).Name}.")
                player.CurrentMana = player.MaximumMana
                PushUIState(UIState.InPlay)
                Return UIState.Message
        End Select
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As ICharacter) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
