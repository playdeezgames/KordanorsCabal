Friend Class ShoppeOffersProcessor
    Inherits ShoppeProcessor(Of String)

    Public Overrides Sub UpdateBuffer(buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Offers", True, Hue.Blue)

        For row = ListStartRow To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                buffer.WriteTextCentered(row, items(itemIndex), itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Esc", False, Hue.Black)
    End Sub

    Public Overrides Sub Initialize()
        currentItemIndex = 0
        items = ShoppeType.Offers.Select(Function(x) $"{x.Key.Name}: {x.Value}").ToList
    End Sub

    Public Overrides Function ProcessCommand(command As Command) As UIState
        Select Case command
            Case Command.Red
                Return UIState.InPlay
            Case Command.Down
                currentItemIndex = (currentItemIndex + 1) Mod items.Count
            Case Command.Up
                currentItemIndex = (currentItemIndex + items.Count - 1) Mod items.Count
        End Select
        Return UIState.ShoppeOffers
    End Function
End Class
