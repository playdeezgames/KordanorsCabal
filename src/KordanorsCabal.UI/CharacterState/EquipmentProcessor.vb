Friend Class EquipmentProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Black)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Equipment", True, Hue.Blue)
        Dim row As Integer = 1
        Dim player = World.PlayerCharacter
        For Each entry In player.Equipment
            buffer.WriteText((0, row), $"{entry.Key.Name}: {entry.Value.Name}", False, Hue.Black)
            row += 1
        Next
        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Return UIState.InPlay
    End Function
End Class
