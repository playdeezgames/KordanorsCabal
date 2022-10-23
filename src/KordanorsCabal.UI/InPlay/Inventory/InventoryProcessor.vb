Imports SPLORR.Game

Friend Class InventoryProcessor
    Inherits BaseProcessor

    Private items As List(Of (String, IEnumerable(Of IItem)))
    Private currentItemIndex As Integer = 0
    Const ListStartRow = 2
    Const ListHiliteRow = 10
    Const ListEndRow = 21

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Inventory", True, Hue.Blue)
        Dim player = World.FromWorldData(worldData).PlayerCharacter
        buffer.WriteTextCentered(1, $"Encumbrance: {player.Encumbrance.CurrentEncumbrance}/{player.Encumbrance.MaximumEncumbrance}", False, Hue.Red)
        For row = ListStartRow To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                buffer.WriteTextCentered(row, $"{items(itemIndex).Item1}({items(itemIndex).Item2.Count})", itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Overrides Sub Initialize()

        currentItemIndex = 0
        items = New List(Of (String, IEnumerable(Of IItem)))
        Dim table As New Dictionary(Of String, List(Of IItem))
        For Each item In World.FromWorldData(WorldData).PlayerCharacter.Items.Inventory.Items.OrderBy(Function(x) x.Name)
            If Not table.ContainsKey(item.Name) Then
                table(item.Name) = New List(Of IItem)
            End If
            table(item.Name).Add(item)
        Next
        For Each entry In table
            items.Add((entry.Key, entry.Value))
        Next
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
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
