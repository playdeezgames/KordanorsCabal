Imports KordanorsCabal.Data

Friend Class EquipmentProcessor
    Inherits BaseProcessor

    Private rowIndex As Integer = 0
    Private table As New Dictionary(Of Integer, IEquipSlot)

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Black)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Equipment", True, Hue.Blue)
        buffer.WriteText((0, 1), "Go Back", rowIndex = 0, Hue.Black)
        Dim row As Integer = 1
        Dim player = Game.World.PlayerCharacter(worldData)
        For Each entry In player.Equipment.EquippedSlots
            Dim slotName = $"{entry.Name}: "
            buffer.WriteText((0, row + 1), slotName, rowIndex = row, Hue.Black)
            Dim item = player.Equipment.CurrentEquipment(entry)
            Dim condition = item.Durability.Maximum / item.Durability.Current
            Dim conditionHue = If(condition >= 4, Hue.Red, If(condition >= 2, Hue.Yellow, Hue.Black))
            buffer.WriteText((slotName.Length, row + 1), item.Name, rowIndex = row, conditionHue)
            row += 1
        Next
        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Overrides Sub Initialize()

        rowIndex = 0
        table.Clear()
        table(0) = Nothing
        Dim row As Integer = 1
        For Each entry In Game.World.PlayerCharacter(WorldData).Equipment.EquippedSlots
            table(row) = entry
            row += 1
        Next
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
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

    Private Function InteractEquipSlot(equipSlot As IEquipSlot) As UIState
        If equipSlot Is Nothing Then
            Return UIState.InPlay
        End If
        EquipmentDetailProcessor.EquipSlot = equipSlot
        Return UIState.EquipmentDetail
    End Function
End Class
