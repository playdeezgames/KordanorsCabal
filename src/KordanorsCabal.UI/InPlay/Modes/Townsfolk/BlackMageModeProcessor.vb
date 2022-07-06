﻿Friend Class BlackMageModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const OffersButtonIndex = 1
    Const SellButtonIndex = 2
    Const GoodByeButtonIndex = 9


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
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case OffersButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.Trophy
                Return UIState.ShoppeOffers
            Case SellButtonIndex
                ShoppeProcessor(Of Item).ShoppeType = Game.ShoppeType.Trophy
                Return UIState.ShoppeSell
        End Select
        Return UIState.InPlay
    End Function
End Class
