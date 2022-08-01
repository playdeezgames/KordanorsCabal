Friend Class InnKeeperModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const QuestButtonIndex = 1
    Const GoodByeButtonIndex = 9
    Const PricesButtonIndex = 6
    Const BuyButtonIndex = 7


    Friend Overrides Sub UpdateBuffer(player As Character, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "@#$% off, you %$#^!", False, Hue.Black)
            Case QuestButtonIndex
                If player.HasQuest(Quest.CellarRats) Then
                    buffer.WriteText((0, 1), "How the rat killing going?", False, Hue.Black)
                    Exit Select
                End If
                buffer.WriteText((0, 1), "Do a guy a favor and kill the rats in the cellar? I'll pay 1 money for 1 rat tail, up to 10 rat tails.", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "@#$% You!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As Character)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(PricesButtonIndex).Title = "Prices"
        Buttons(BuyButtonIndex).Title = "Buy"
        UpdateQuestButton(player)
    End Sub

    Private Sub UpdateQuestButton(player As Character)
        If player.HasQuest(Quest.CellarRats) Then
            Buttons(QuestButtonIndex).Title = "Quest Done!"
            Return
        End If
        If player.CanAcceptQuest(Quest.CellarRats) Then
            Buttons(QuestButtonIndex).Title = "Do Quest!"
            Return
        End If
        Buttons(QuestButtonIndex).Title = ""
    End Sub

    Friend Overrides Function HandleButton(player As Character, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case QuestButtonIndex
                Return HandleQuestButton(player)
            Case PricesButtonIndex
                ShoppeProcessor(Of String).ShoppeType = Game.ShoppeType.InnKeeper
                Return UIState.ShoppePrices
            Case BuyButtonIndex
                ShoppeProcessor(Of (ItemType, Long)).ShoppeType = Game.ShoppeType.InnKeeper
                Return UIState.ShoppeBuy
        End Select
        Return UIState.InPlay
    End Function

    Private Function HandleQuestButton(player As Character) As UIState
        If player.HasQuest(Quest.CellarRats) Then
            player.CompleteQuest(Quest.CellarRats)
            PushUIState(UIState.InPlay)
            Return UIState.Message
        End If
        If player.CanAcceptQuest(Quest.CellarRats) Then
            player.AcceptQuest(Quest.CellarRats)
            PushUIState(UIState.InPlay)
            Return UIState.Message
        End If
        Return UIState.InPlay
    End Function

    Friend Overrides Function HandleRed(player As Character) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
