Imports SPLORR.Game

Friend Class ChickenModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9
    Const FeedButtonIndex = 5


    Friend Overrides Sub UpdateBuffer(player As Character, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "MOO! I'm a cow!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "MOO! I'm a cow!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As Character)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        If player.HasItemType(ItemType.Food) OrElse player.HasItemType(ItemType.RottenFood) Then
            Buttons(FeedButtonIndex).Title = "Feed"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As Character, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case FeedButtonIndex
                If player.HasItemType(ItemType.Food) OrElse player.HasItemType(ItemType.RottenFood) Then
                    Dim item = RNG.FromEnumerable(player.Inventory.Items.Where(Function(x) x.Name = ItemType.Food.Name))
                    Return FeedChicken(player, item)
                End If
        End Select
        Return UIState.InPlay
    End Function

    Private Function FeedChicken(player As Character, item As Item) As UIState
        Dim itemType = item.ItemType
        item.Destroy()
        If RNG.FromRange(0, 5) = 0 Then
            Select Case itemType
                Case ItemType.Food
                    player.EnqueueMessage($"{OldFeatureType.Chicken.Name} eats the food and then a {ItemType.MagicEgg.Name} pops out!")
                    player.Inventory.Add(Item.Create(ItemType.MagicEgg))
                Case ItemType.RottenFood
                    player.EnqueueMessage($"{OldFeatureType.Chicken.Name} eats the rotten food and then a {ItemType.RottenEgg.Name} pops out!")
                    player.Inventory.Add(Item.Create(ItemType.RottenEgg))
            End Select
        Else
            player.EnqueueMessage($"{OldFeatureType.Chicken.Name} eats the food, and gives a satified ""moo"" in return.")
        End If
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function

    Friend Overrides Function HandleRed(player As Character) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
