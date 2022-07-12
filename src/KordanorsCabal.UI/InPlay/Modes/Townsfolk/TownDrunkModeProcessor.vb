Friend Class TownDrunkModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9
    Const EnableButtonIndex = 5


    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "*HIC*", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "*HIC*", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        If player.HasItemType(ItemType.Beer) Then
            Buttons(EnableButtonIndex).Title = "Give Beer"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case EnableButtonIndex
                Return GiveBeer(player)
        End Select
        Return UIState.InPlay
    End Function

    Private Function GiveBeer(player As PlayerCharacter) As UIState
        player.EnqueueMessage($"You give {ItemType.Beer.Name} to {FeatureType.TownDrunk.Name}.", $"{FeatureType.TownDrunk.Name} drinks it all in one swallow, burps, and hands you {ItemType.Bottle.Name}.")
        player.Inventory.ItemsOfType(ItemType.Beer).First.Destroy()
        player.Inventory.Add(Item.Create(ItemType.Bottle))
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function
End Class
