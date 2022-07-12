Imports SPLORR.Game

Friend Class ChickenModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9
    Const FeedButtonIndex = 5


    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "MOO! I'm a cow!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "MOO! I'm a cow!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        If player.HasItemType(ItemType.Food) Then
            Buttons(FeedButtonIndex).Title = "Feed"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case FeedButtonIndex
                If player.HasItemType(ItemType.Food) Then
                    Return FeedChicken(player)
                End If
        End Select
        Return UIState.InPlay
    End Function

    Private Function FeedChicken(player As PlayerCharacter) As UIState
        player.Inventory.ItemsOfType(ItemType.Food).First.Destroy()
        If RNG.FromRange(0, 5) = 0 Then
            player.EnqueueMessage($"{FeatureType.Chicken.Name} and then a {ItemType.MagicEgg.Name} pops out!")
            player.Inventory.Add(Item.Create(ItemType.MagicEgg))
        Else
            player.EnqueueMessage($"{FeatureType.Chicken.Name} eats the food, and gives a satified ""moo"" in return.")
        End If
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function
End Class
