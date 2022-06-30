Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnButtonIndex = 0
    Const InteractButtonIndex = 1
    Const GroundButtonIndex = 2

    Const MapButtonIndex = 4
    Const MoveButtonIndex = 5
    Const InventoryButtonIndex = 6

    Const MenuButtonIndex = 9

    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        Dim location = player.Location
        Select Case location.LocationType
            Case LocationType.Dungeon, LocationType.DungeonDeadEnd, LocationType.DungeonBoss
                ShowDungeon(buffer, player)
            Case Else
                ShowHeader(buffer, location.Name)
                ShowFacing(buffer, (0, 1), player)
                ShowExits(buffer, (0, 2), player)
                ShowFeatures(buffer, (0, 3), player)
        End Select
    End Sub
    Private Sub ShowFeatures(buffer As PatternBuffer, xy As (Integer, Integer), player As PlayerCharacter)
        Dim feature = player.Location.Feature
        If feature IsNot Nothing Then
            buffer.WriteText(xy, $"You see: {feature.Name}", False, Hue.Black)
        End If
    End Sub
    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(TurnButtonIndex).Title = If(player.CanFight, "FIGHT!", "Turn...")
        Buttons(MoveButtonIndex).Title = "Move..."
        Buttons(MenuButtonIndex).Title = "Game Menu"
        Buttons(MapButtonIndex).Title = "Map"
        If player.CanInteract Then
            Buttons(InteractButtonIndex).Title = "Interact..."
        End If
        If Not player.Location.Inventory.IsEmpty Then
            Buttons(GroundButtonIndex).Title = "Ground..."
        End If
        If Not player.Inventory.IsEmpty Then
            Buttons(InventoryButtonIndex).Title = "Inventory"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case TurnButtonIndex 'also the fight button!
                If player.CanFight Then
                    player.Fight()
                    Return UIState.Message
                End If
                PushButtonIndex(0)
                player.Mode = PlayerMode.Turn
            Case MoveButtonIndex
                PushButtonIndex(0)
                player.Mode = PlayerMode.Move
            Case InteractButtonIndex
                If player.CanInteract Then
                    PushButtonIndex(0)
                    player.Interact()
                    Return UIState.InPlay
                End If
            Case GroundButtonIndex
                If Not player.Location.Inventory.IsEmpty Then
                    Return UIState.GroundInventory
                End If
            Case MenuButtonIndex
                Return UIState.GameMenuScreen
            Case InventoryButtonIndex
                If Not player.Inventory.IsEmpty Then
                    Return UIState.Inventory
                End If
            Case MapButtonIndex
                Return UIState.Map
        End Select
        Return UIState.InPlay
    End Function
End Class
