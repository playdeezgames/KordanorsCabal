Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnFightButtonIndex = 0
    Const InteractIntimidateButtonIndex = 1
    Const GroundEnemiesButtonIndex = 2
    Const StatusLevelUpButtonIndex = 3
    Const MapButtonIndex = 4
    Const MoveRunButtonIndex = 5
    Const InventoryButtonIndex = 6
    '7 Equipment?
    '8
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
        Buttons(TurnFightButtonIndex).Title = If(player.CanFight, "FIGHT!", "Turn...")
        Buttons(MoveRunButtonIndex).Title = If(player.CanFight, "RUN!", "Move...")
        Buttons(MenuButtonIndex).Title = "Game Menu"
        Buttons(MapButtonIndex).Title = "Map"
        Buttons(StatusLevelUpButtonIndex).Title = If(player.IsFullyAssigned, "Status", "Level up!")
        If player.CanInteract Then
            Buttons(InteractIntimidateButtonIndex).Title = "Interact..."
        End If
        Dim enemyCount = player.Location.Enemies(player).Count
        Buttons(GroundEnemiesButtonIndex).Title =
            If(player.CanFight,
                $"Enemies({enemyCount})",
                If(Not player.Location.Inventory.IsEmpty,
                    "Ground...",
                    ""))
        If Not player.Inventory.IsEmpty Then
            Buttons(InventoryButtonIndex).Title = "Inventory"
        End If
    End Sub

    Friend Overrides Function HandleButton(player As PlayerCharacter, button As Button) As UIState
        Select Case button.Index
            Case TurnFightButtonIndex 'also the fight button!
                If player.CanFight Then
                    player.Fight()
                    MainProcessor.PushUIState(UIState.InPlay)
                    Return UIState.Message
                End If
                PushButtonIndex(0)
                player.Mode = PlayerMode.Turn
            Case MoveRunButtonIndex
                If player.CanFight Then
                    player.Run()
                    MainProcessor.PushUIState(UIState.InPlay)
                    Return UIState.Message
                End If
                PushButtonIndex(0)
                player.Mode = PlayerMode.Move
            Case InteractIntimidateButtonIndex
                If player.CanInteract Then
                    PushButtonIndex(0)
                    player.Interact()
                    Return UIState.InPlay
                End If
            Case GroundEnemiesButtonIndex
                If player.CanFight Then
                    Return UIState.Enemies
                End If
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
            Case StatusLevelUpButtonIndex
                If Not player.IsFullyAssigned Then
                    Return UIState.LevelUp
                End If
                Return UIState.Status
        End Select
        Return UIState.InPlay
    End Function
End Class
