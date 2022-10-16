Friend Class HealerModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const HealButtonIndex = 1

    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7

    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As IPlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Movement.Location.Feature.Name)
        Const defaultResponse As String = "What ails you, my friend?"
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case HealButtonIndex
                If player.Health.NeedsHealing Then
                    buffer.WriteText((0, 1), "Any time yer ready!", False, Hue.Black)
                Else
                    buffer.WriteText((0, 1), defaultResponse, False, Hue.Black)
                End If
            Case Else
                buffer.WriteText((0, 1), defaultResponse, False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As IPlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        If player.Health.NeedsHealing Then
            Buttons(HealButtonIndex).Title = "Heal me!"
        End If
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
    End Sub

    Friend Overrides Function HandleButton(player As IPlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case HealButtonIndex
                player.Health.Heal()
            Case PricesButtonIndex
                ShoppeProcessor(Of String).ShoppeType = ShoppeType.FromId(StaticWorldData.World, 4)
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                ShoppeProcessor(Of (Long, Long)).ShoppeType = ShoppeType.FromId(StaticWorldData.World, 4)
                Return UIState.ShoppeBuy
        End Select
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As IPlayerCharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
