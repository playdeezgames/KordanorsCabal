Imports KordanorsCabal.Data
Imports SPLORR.Game

Friend Class InventoryProcessor
    Implements IProcessor

    Private items As List(Of (String, IEnumerable(Of Item)))
    Private currentItemIndex As Integer = 0
    Const ListStartRow = 2
    Const ListHiliteRow = 10
    Const ListEndRow = 21

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Inventory", True, Hue.Blue)
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)
        buffer.WriteTextCentered(1, $"Encumbrance: {player.Encumbrance}/{player.MaximumEncumbrance}", False, Hue.Red)
        For row = ListStartRow To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                buffer.WriteTextCentered(row, $"{items(itemIndex).Item1}({items(itemIndex).Item2.Count})", itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
        currentItemIndex = 0
        items = New List(Of (String, IEnumerable(Of Item)))
        Dim table As New Dictionary(Of String, List(Of Item))
        For Each item In Game.World.PlayerCharacter(StaticWorldData.World).Inventory.Items.OrderBy(Function(x) x.Name)
            If Not table.ContainsKey(item.Name) Then
                table(item.Name) = New List(Of Item)
            End If
            table(item.Name).Add(item)
        Next
        For Each entry In table
            items.Add((entry.Key, entry.Value))
        Next
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Red
                Return UIState.InPlay
            Case Command.Down
                currentItemIndex = (currentItemIndex + 1) Mod items.Count
            Case Command.Up
                currentItemIndex = (currentItemIndex + items.Count - 1) Mod items.Count
            Case Command.Green, Command.Blue
                Return InteractItem()
        End Select
        Return UIState.Inventory
    End Function

    Private Function InteractItem() As UIState
        InteractItemProcessor.InteractItem = RNG.FromEnumerable(items(currentItemIndex).Item2)
        Return UIState.InteractItem
    End Function
End Class
