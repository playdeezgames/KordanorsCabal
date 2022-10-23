﻿Imports KordanorsCabal.Data

Friend Class InteractItemProcessor
    Inherits MenuProcessor

    Const CancelMenuItem = "Cancel"
    Const DropMenuItem = "Drop"
    Const UseMenuItem = "Use"
    Const EquipMenuItem = "Equip"


    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                (CancelMenuItem, Function() UIState.Inventory),
                (DropMenuItem, AddressOf DropItem),
                (UseMenuItem, AddressOf UseItem),
                (EquipMenuItem, AddressOf EquipItem)
            },
            5,
            UIState.InteractItem)
    End Sub

    Private Shared Function EquipItem() As UIState
        Game.StaticWorld.PlayerCharacter(WorldData).Equipment.Equip(InteractItem)
        MainProcessor.PushUIState(UIState.Inventory)
        Return UIState.Message
    End Function

    Private Shared Function UseItem() As UIState
        If InteractItem.Usage.CanUse(Game.StaticWorld.PlayerCharacter(WorldData)) Then
            Game.StaticWorld.PlayerCharacter(WorldData).Items.UseItem(InteractItem)
            MainProcessor.PushUIState(UIState.Inventory)
            Return UIState.Message
        End If
        Game.StaticWorld.PlayerCharacter(WorldData).EnqueueMessage(Nothing, "You cannot use that now!") 'TODO: shucks!
        MainProcessor.PushUIState(UIState.InteractItem)
        Return UIState.Message
    End Function

    Private Shared Function DropItem() As UIState
        Game.StaticWorld.PlayerCharacter(WorldData).Movement.Location.Inventory.Add(InteractItem)
        If Game.StaticWorld.PlayerCharacter(WorldData).Items.Inventory.IsEmpty Then
            Return UIState.InPlay
        End If
        Return UIState.Inventory
    End Function

    Public Shared Property InteractItem As IItem

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(1, $"Encumbrance: {InteractItem.ItemType.Encumbrance}", False, Hue.Black)
        buffer.WriteTextCentered(0, InteractItem.Name, True, Hue.Blue)
    End Sub

    Public Overrides Sub Initialize()

        currentItem = 0
    End Sub

    Public Overrides Function HandleRed() As UIState
        Return UIState.Inventory
    End Function
End Class
