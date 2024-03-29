﻿Imports SPLORR.Game

Friend Class ChickenModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9
    Const FeedButtonIndex = 5


    Friend Overrides Sub UpdateBuffer(player As IPlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Movement.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "MOO! I'm a cow!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "MOO! I'm a cow!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As IPlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        If player.Items.HasItemType(ItemType.FromId(WorldData, ItemType24)) OrElse player.Items.HasItemType(ItemType.FromId(WorldData, ItemType35)) Then
            Buttons(FeedButtonIndex).Title = "Feed"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As IPlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case FeedButtonIndex
                If player.Items.HasItemType(ItemType.FromId(WorldData, ItemType24)) OrElse player.Items.HasItemType(ItemType.FromId(WorldData, ItemType35)) Then
                    Dim item = RNG.FromEnumerable(player.Items.Inventory.Items.Where(Function(x) x.Name = ItemType.FromId(WorldData, ItemType24).Name))
                    Return FeedChicken(player, item)
                End If
        End Select
        Return UIState.InPlay
    End Function

    Private Function FeedChicken(player As ICharacter, item As IItem) As UIState
        Dim itemType = item.ItemType
        item.Destroy()
        If RNG.FromRange(0, 5) = 0 Then
            Select Case itemType.Id
                Case 24L
                    player.EnqueueMessage(Nothing, $"{New FeatureType(WorldData, 4).Name} eats the food and then a {Game.ItemType.FromId(WorldData, ItemType25).Name} pops out!")
                    player.Items.Inventory.Add(Game.Item.Create(WorldData, Game.ItemType.FromId(WorldData, ItemType25)))
                Case 35L
                    player.EnqueueMessage(Nothing, $"{New FeatureType(WorldData, 4L).Name} eats the rotten food and then a {Game.ItemType.FromId(WorldData, ItemType37).Name} pops out!")
                    player.Items.Inventory.Add(Game.Item.Create(WorldData, Game.ItemType.FromId(WorldData, ItemType37)))
            End Select
        Else
            player.EnqueueMessage(Nothing, $"{New FeatureType(WorldData, 4L).Name} eats the food, and gives a satified ""moo"" in return.")
        End If
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function

    Friend Overrides Function HandleRed(player As IPlayerCharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
