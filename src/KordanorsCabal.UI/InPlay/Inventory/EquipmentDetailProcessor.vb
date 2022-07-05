Friend Class EquipmentDetailProcessor
    Inherits MenuProcessor

    Public Shared Property EquipSlot As EquipSlot

    Const GoBackMenuItem = "Go Back"
    Const UnequipMenuItem = "Unequip"

    Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                (GoBackMenuItem, Function() UIState.Equipment),
                (UnequipMenuItem, AddressOf Unequip)
            },
            14,
            UIState.EquipmentDetail)
    End Sub

    Private Shared Function Unequip() As UIState
        Dim player = World.PlayerCharacter
        player.Unequip(EquipSlot)
        If player.Equipment.Any Then
            Return UIState.Equipment
        End If
        Return UIState.InPlay
    End Function

    Public Overrides Sub Initialize()
        currentItem = 0
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        Dim item = World.PlayerCharacter.Equipment(EquipSlot)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, EquipSlot.Name, True, Hue.Blue)
        buffer.WriteText((0, 1), $"Item: {item.Name}", False, Hue.Black)
        If item.Durability.HasValue Then
            buffer.WriteText((0, 2), $"Durability: {item.Durability.Value}/{item.MaximumDurability.Value}", False, Hue.Black)
        End If
    End Sub
End Class
