﻿Imports SPLORR.Data

Friend Class SaveGameScreenProcessor
    Inherits MenuProcessor
    Public Shared Property Validated As Boolean = False
    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Go Back", Function() UIState.GameMenuScreen),
                ("Slot 1", Function() SaveSlot(1)),
                ("Slot 2", Function() SaveSlot(2)),
                ("Slot 3", Function() SaveSlot(3)),
                ("Slot 4", Function() SaveSlot(4)),
                ("Slot 5", Function() SaveSlot(5))
            },
            5,
            UIState.SaveGameScreen)
    End Sub

    Private Shared Function SaveSlot(slotNumber As Integer) As UIState
        Store.Save(SaveSlotName(slotNumber))
        Return UIState.InPlay
    End Function

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, "Save Game", False, Hue.Blue)
        ValidateSlots()
    End Sub

    Friend Sub ValidateSlots()
        If Not Validated Then
            Dim tempFilename = $"{Guid.NewGuid}.db"
            Store.Save(tempFilename)
            ValidateSlot(1)
            ValidateSlot(2)
            ValidateSlot(3)
            ValidateSlot(4)
            ValidateSlot(5)
            Store.Load(tempFilename)
            Validated = True
        End If
    End Sub

    Private Sub ValidateSlot(slotNumber As Integer)
        Store.Load(SaveSlotName(slotNumber))
        If World.IsValid Then
            UpdateMenuItemText(slotNumber, $"Slot {slotNumber}")
        Else
            UpdateMenuItemText(slotNumber, "(empty)")
        End If
    End Sub
End Class