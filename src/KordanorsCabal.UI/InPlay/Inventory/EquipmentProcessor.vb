Friend Class EquipmentProcessor
    Implements IProcessor

    Private rowIndex As Integer = 0
    Private table As New Dictionary(Of Integer, EquipSlot)

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Black)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Equipment", True, Hue.Blue)
        buffer.WriteText((0, 1), "Go Back", rowIndex = 0, Hue.Black)
        Dim row As Integer = 1
        Dim player = World.PlayerCharacter
        For Each entry In player.EquippedSlots
            Dim slotName = $"{entry.Name}: "
            buffer.WriteText((0, row + 1), slotName, rowIndex = row, Hue.Black)
            Dim item = player.Equipment(entry)
            Dim condition = item.MaximumDurability / item.Durability
            Dim conditionHue = If(condition >= 4, Hue.Red, If(condition >= 2, Hue.Yellow, Hue.Black))
            buffer.WriteText((slotName.Length, row + 1), item.Name, rowIndex = row, conditionHue)
            row += 1
        Next
        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
        rowIndex = 0
        table.Clear()
        table(0) = EquipSlot.None
        Dim row As Integer = 1
        For Each entry In World.PlayerCharacter.EquippedSlots
            table(row) = entry.ToEquipSlot
            row += 1
        Next
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Red
                Return UIState.InPlay
            Case Command.Up
                rowIndex = (rowIndex + table.Count - 1) Mod table.Count
            Case Command.Down
                rowIndex = (rowIndex + 1) Mod table.Count
            Case Command.Green, Command.Blue
                Return InteractEquipSlot(table(rowIndex))
        End Select
        Return UIState.Equipment
    End Function

    Private Function InteractEquipSlot(equipSlot As EquipSlot) As UIState
        If equipSlot = EquipSlot.None Then
            Return UIState.InPlay
        End If
        EquipmentDetailProcessor.EquipSlot = equipSlot
        Return UIState.EquipmentDetail
    End Function
End Class
