﻿Friend Class InteractItemProcessor
    Inherits MenuProcessor

    Const CancelMenuItem = "Cancel"
    Const DropMenuItem = "Drop"


    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                (CancelMenuItem, Function() UIState.Inventory),
                (DropMenuItem, AddressOf DropItem)
            },
            5,
            UIState.InteractItem)
    End Sub

    Private Shared Function DropItem() As UIState
        World.PlayerCharacter.Location.Inventory.Add(InteractItem)
        If World.PlayerCharacter.Inventory.IsEmpty Then
            Return UIState.InPlay
        End If
        Return UIState.Inventory
    End Function

    Public Shared Property InteractItem As Item

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, InteractItem.Name, True, Hue.Blue)
    End Sub

    Public Overrides Sub Initialize()
        currentItem = 0
    End Sub
End Class