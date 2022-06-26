﻿Friend Class GroundInventoryProcessor
    Implements IProcessor

    Private groundItems As List(Of Item)
    Private currentItemIndex As Integer = 0
    Const ListStartRow = 1
    Const ListHiliteRow = 10
    Const ListEndRow = 21

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "On the Ground", True, Hue.Blue)

        For row = ListStartRow To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < groundItems.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                buffer.WriteTextCentered(row, groundItems(itemIndex).Name, itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
        currentItemIndex = 0
        groundItems = World.PlayerCharacter.Location.Inventory.Items.ToList
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Red
                Return UIState.InPlay
            Case Else
                Return UIState.GroundInventory
        End Select
    End Function
End Class
