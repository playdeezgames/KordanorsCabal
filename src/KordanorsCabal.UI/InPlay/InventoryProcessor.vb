Friend Class InventoryProcessor
    Implements IProcessor

    Private items As List(Of Item)
    Private currentItemIndex As Integer = 0
    Const ListStartRow = 1
    Const ListHiliteRow = 10
    Const ListEndRow = 21

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Inventory", True, Hue.Blue)

        For row = ListStartRow To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                buffer.WriteTextCentered(row, items(itemIndex).Name, itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
        currentItemIndex = 0
        items = World.PlayerCharacter.Inventory.Items.ToList
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Red
                Return UIState.InPlay
            Case Command.Up
                currentItemIndex = (currentItemIndex + 1) Mod items.Count
            Case Command.Down
                currentItemIndex = (currentItemIndex + items.Count - 1) Mod items.Count
            Case Command.Green, Command.Blue
                Return PickUpItem()
        End Select
        Return UIState.Inventory
    End Function

    Private Function PickUpItem() As UIState
        Throw New NotImplementedException
    End Function
End Class
