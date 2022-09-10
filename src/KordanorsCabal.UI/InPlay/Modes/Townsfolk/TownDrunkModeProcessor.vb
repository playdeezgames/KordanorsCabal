Imports KordanorsCabal.Data

Friend Class TownDrunkModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9
    Const EnableButtonIndex = 5


    Friend Overrides Sub UpdateBuffer(player As ICharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "*HIC*", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "*HIC*", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As ICharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
        If player.HasItemType(OldItemType.Beer) Then
            Buttons(EnableButtonIndex).Title = "Give Beer"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As ICharacter, button As Button) As UIState
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
            Case EnableButtonIndex
                If player.HasItemType(OldItemType.Beer) Then
                    Return GiveBeer(player)
                End If
        End Select
        Return UIState.InPlay
    End Function

    Private Function GiveBeer(player As ICharacter) As UIState
        player.EnqueueMessage($"You give {OldItemType.Beer.Name} to {New FeatureType(StaticWorldData.World, 3L).Name}.", $"{New FeatureType(StaticWorldData.World, 3L).Name} drinks it all in one swallow, burps, and hands you {OldItemType.Bottle.Name}.")
        player.Inventory.ItemsOfType(OldItemType.Beer).First.Destroy()
        player.Inventory.Add(Item.Create(StaticWorldData.World, OldItemType.Bottle))
        PushUIState(UIState.InPlay)
        Return UIState.Message
    End Function

    Friend Overrides Function HandleRed(player As ICharacter) As UIState
        player.Mode = PlayerMode.Neutral
        Return UIState.InPlay
    End Function
End Class
