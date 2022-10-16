Friend Class NeutralModeProcessor
    Inherits ModeProcessor

    Const TurnFightButtonIndex = 0
    Const InteractIntimidateButtonIndex = 1
    Const GroundEnemiesButtonIndex = 2
    Const StatusLevelUpButtonIndex = 3
    Const MapButtonIndex = 4
    Const MoveRunButtonIndex = 5
    Const InventoryButtonIndex = 6
    Const EquipmentButtonIndex = 7
    Const SpellsButtonIndex = 8
    Const MenuButtonIndex = 9

    Friend Overrides Sub UpdateBuffer(player As ICharacter, buffer As PatternBuffer)
        Dim location = player.Movement.Location
        If location.LocationType.IsDungeon Then
            ShowDungeon(buffer, player)
        Else
            ShowHeader(buffer, location.LocationType.Name)
            ShowFacing(buffer, (0, 1), player)
            ShowExits(buffer, (0, 2), player)
            ShowFeatures(buffer, (0, 3), player)
            ShowEncumbered(buffer, (0, 5), player)
            ShowTownPortal(buffer, (0, 7), player)
        End If
    End Sub

    Private Sub ShowTownPortal(buffer As PatternBuffer, xy As (Integer, Integer), player As ICharacter)
        If player.Movement.Location.Routes.RouteTypes.Any(Function(x) x.Id = 10L) Then
            buffer.WriteText(xy, "There is a portal here!", False, Hue.Purple)
        End If
    End Sub

    Private Sub ShowEncumbered(buffer As PatternBuffer, xy As (Integer, Integer), player As ICharacter)
        If player.Encumbrance.IsEncumbered Then
            buffer.WriteText(xy, "You are encumbered!", False, Hue.Red)
        End If
    End Sub

    Private Sub ShowFeatures(buffer As PatternBuffer, xy As (Integer, Integer), player As ICharacter)
        Dim feature = player.Movement.Location.Feature
        If feature IsNot Nothing Then
            buffer.WriteText(xy, $"You see: {feature.Name}", False, Hue.Black)
        End If
    End Sub
    Friend Overrides Sub UpdateButtons(player As ICharacter)
        Buttons(TurnFightButtonIndex).Title = If(player.PhysicalCombat.CanFight, "FIGHT!", "Turn...")
        Buttons(MoveRunButtonIndex).Title = If(player.PhysicalCombat.CanFight, "RUN!", "Move...")
        Buttons(MenuButtonIndex).Title = "Game Menu"
        If player.Spellbook.HasSpells Then
            Buttons(SpellsButtonIndex).Title = "Spells"
        End If
        If player.Movement.CanMap Then
            Buttons(MapButtonIndex).Title = "Map"
        End If
        If player.Equipment.HasEquipment Then
            Buttons(EquipmentButtonIndex).Title = "Equipment"
        End If
        Buttons(StatusLevelUpButtonIndex).Title = If(player.Advancement.IsFullyAssigned, "Status", "Level up!")
        If player.MentalCombat.CanDoIntimidation Then
            Buttons(InteractIntimidateButtonIndex).Title = "Intimidate!"
        ElseIf player.Interaction.CanInteract Then
            Buttons(InteractIntimidateButtonIndex).Title = "Interact..."
        End If
        Dim enemyCount = player.Movement.Location.Factions.EnemiesOf(player).Count
        Buttons(GroundEnemiesButtonIndex).Title =
            If(player.PhysicalCombat.CanFight,
                $"Enemies({enemyCount})",
                If(Not player.Movement.Location.Inventory.IsEmpty,
                    "Ground...",
                    ""))
        If Not player.Items.Inventory.IsEmpty Then
            Buttons(InventoryButtonIndex).Title = "Inventory"
        End If
    End Sub
    Friend Overrides Function HandleButton(player As ICharacter, button As Button) As UIState
        Select Case button.Index
            Case TurnFightButtonIndex 'also the fight button!
                If player.PhysicalCombat.CanFight Then
                    player.PhysicalCombat.Fight()
                    MainProcessor.PushUIState(UIState.InPlay)
                    Return UIState.Message
                End If
                PushButtonIndex(0)
                player.Mode = Game.Constants.PlayerModes.Turn
            Case MoveRunButtonIndex
                If player.PhysicalCombat.CanFight Then
                    player.PhysicalCombat.Run()
                    MainProcessor.PushUIState(UIState.InPlay)
                    Return UIState.Message
                End If
                PushButtonIndex(0)
                player.Mode = Game.Constants.PlayerModes.Move
            Case InteractIntimidateButtonIndex
                If player.MentalCombat.CanDoIntimidation Then
                    player.MentalCombat.DoIntimidation()
                    MainProcessor.PushUIState(UIState.InPlay)
                    Return UIState.Message
                ElseIf player.Interaction.CanInteract Then
                    PushButtonIndex(0)
                    player.Interaction.Interact()
                    Return UIState.InPlay
                End If
            Case GroundEnemiesButtonIndex
                If player.PhysicalCombat.CanFight Then
                    Return UIState.Enemies
                End If
                If Not player.Movement.Location.Inventory.IsEmpty Then
                    Return UIState.GroundInventory
                End If
            Case MenuButtonIndex
                Return UIState.GameMenuScreen
            Case InventoryButtonIndex
                If Not player.Items.Inventory.IsEmpty Then
                    Return UIState.Inventory
                End If
            Case MapButtonIndex
                If player.Movement.CanMap Then
                    Return UIState.Map
                End If
            Case EquipmentButtonIndex
                If player.Equipment.HasEquipment Then
                    Return UIState.Equipment
                End If
            Case StatusLevelUpButtonIndex
                If Not player.Advancement.IsFullyAssigned Then
                    Return UIState.LevelUp
                End If
                Return UIState.Status
            Case SpellsButtonIndex
                If player.Spellbook.HasSpells Then
                    Return UIState.SpellList
                End If
        End Select
        Return UIState.InPlay
    End Function
    Friend Overrides Function HandleRed(player As ICharacter) As UIState
        player.Mode = Game.Constants.PlayerModes.Neutral
        Return UIState.InPlay
    End Function
End Class
