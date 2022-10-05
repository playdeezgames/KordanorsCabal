Friend Class ConstableModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const BountiesButtonIndex = 1
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As ICharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case BountiesButtonIndex
                buffer.WriteText((0, 1), "We've been plagued by a group of malcontents! I will give 10 money for proof that a malcontent has been dealt with.", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Brint it, trolls!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As ICharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        Buttons(BountiesButtonIndex).Title = "Bounties"
    End Sub

    Friend Overrides Function HandleButton(player As ICharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case BountiesButtonIndex
                Return HandleBounties(player)
        End Select
        Return UIState.InPlay
    End Function

    Private Function HandleBounties(player As ICharacter) As UIState
        If player.HasItemType(ItemType.FromId(StaticWorldData.World, 32)) Then
            Dim cards = player.Inventory.ItemsOfType(OldItemType.MembershipCard.ToNew(StaticWorldData.World))
            Dim reward As Long = cards.Count * 10
            player.EnqueueMessage($"I'll be certain to file these under evidence. Here's yer reward! {reward} money.")
            player.Money += reward
            For Each card In cards
                card.Destroy()
            Next
        Else
            player.EnqueueMessage("See me when you've got evidence of malcontent activity.")
        End If
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function

    Friend Overrides Function HandleRed(player As ICharacter) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
