Imports KordanorsCabal.Data

Friend Class TownDrunkModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9
    Const EnableButtonIndex = 5


    Friend Overrides Sub UpdateBuffer(player As IPlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Movement.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "*HIC*", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "*HIC*", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As IPlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        If player.Items.HasItemType(ItemType.FromId(WorldData, ItemType26)) Then
            Buttons(EnableButtonIndex).Title = "Give Beer"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As IPlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = Game.Constants.PlayerModes.Neutral
            Case EnableButtonIndex
                If player.Items.HasItemType(ItemType.FromId(WorldData, ItemType26)) Then
                    Return GiveBeer(player)
                End If
        End Select
        Return UIState.InPlay
    End Function

    Private Function GiveBeer(player As ICharacter) As UIState
        player.EnqueueMessage(Nothing, $"You give {ItemType.FromId(WorldData, ItemType26).Name} to {New FeatureType(WorldData, 3L).Name}.", $"{New FeatureType(WorldData, 3L).Name} drinks it all in one swallow, burps, and hands you {ItemType.FromId(WorldData, ItemType30).Name}.")
        player.Items.Inventory.ItemsOfType(ItemType.FromId(WorldData, ItemType26)).First.Destroy()
        player.Items.Inventory.Add(Item.Create(WorldData, ItemType30))
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function

    Friend Overrides Function HandleRed(player As IPlayerCharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
