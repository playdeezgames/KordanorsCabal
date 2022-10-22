Imports KordanorsCabal.Data
Imports SPLORR.Data

Friend Class LoadGameScreenProcessor
    Inherits MenuProcessor
    Public Shared Property Validated As Boolean = False
    Public Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("Go Back", Function() UIState.TitleScreen),
                ("Slot 1", Function() ContinueSlot(1)),
                ("Slot 2", Function() ContinueSlot(2)),
                ("Slot 3", Function() ContinueSlot(3)),
                ("Slot 4", Function() ContinueSlot(4)),
                ("Slot 5", Function() ContinueSlot(5))
            },
            5,
            UIState.LoadGameScreen)
    End Sub

    Private Shared Function ContinueSlot(slotNumber As Integer) As UIState
        WorldData.Load(SaveSlotName(slotNumber))
        If Game.World.IsValid(WorldData) Then
            Return UIState.InPlay
        End If
        Return TitleScreenProcessor.StartGame()
    End Function

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(0, "Continue Game", False, Hue.Blue)
        ValidateSlots()
    End Sub

    Friend Sub ValidateSlots()
        If Not Validated Then
            ValidateSlot(1)
            ValidateSlot(2)
            ValidateSlot(3)
            ValidateSlot(4)
            ValidateSlot(5)
            Validated = True
        End If
    End Sub

    Private Sub ValidateSlot(slotNumber As Integer)
        WorldData.Load(SaveSlotName(slotNumber))
        If Game.World.IsValid(WorldData) Then
            UpdateMenuItemText(slotNumber, $"Slot {slotNumber}")
        Else
            UpdateMenuItemText(slotNumber, "(empty)")
        End If
    End Sub

    Public Overrides Function HandleRed() As UIState
        Return UIState.TitleScreen
    End Function

    Public Overrides Sub Initialize()
    End Sub
End Class
