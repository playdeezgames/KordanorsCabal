Imports KordanorsCabal.Data

Friend Class ShoppeRepairProcessor
    Inherits ShoppeProcessor(Of IItem)

    Public Overrides Sub UpdateBuffer(buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, $"Repair", True, Hue.Blue)
        buffer.WriteTextCentered(1, $"Money: {Game.World.PlayerCharacter(StaticWorldData.World).Money}", False, Hue.Black)

        For row = ListStartRow + 1 To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                Dim text = $"{items(itemIndex).Name}({items(itemIndex).RepairCost(ShoppeType)})"
                buffer.WriteTextCentered(row, text, itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Overrides Sub Initialize()
        currentItemIndex = 0
        Dim repairs = ShoppeType.Repairs
        items = Game.World.PlayerCharacter(StaticWorldData.World).
            ItemsToRepair(ShoppeType).
            Where(Function(x) ShoppeType.WillRepair(x.ItemType)).ToList
    End Sub

    Public Overrides Function ProcessCommand(command As Command) As UIState
        Select Case command
            Case Command.Red
                Return UIState.InPlay
            Case Command.Down
                If items.Any Then
                    currentItemIndex = (currentItemIndex + 1) Mod items.Count
                End If
            Case Command.Up
                If items.Any Then
                    currentItemIndex = (currentItemIndex + items.Count - 1) Mod items.Count
                End If
            Case Command.Green, Command.Blue
                Return RepairItem
        End Select
        Return UIState.ShoppeRepair
    End Function

    Private Function RepairItem() As UIState
        If Not items.Any Then
            Return UIState.InPlay
        End If
        Dim item = items(currentItemIndex)
        Dim repairCost = item.RepairCost(Game.ShoppeType.FromId(StaticWorldData.World, OldShoppeType.Blacksmith))
        If Game.World.PlayerCharacter(StaticWorldData.World).Money < repairCost Then
            Return UIState.ShoppeRepair
        End If
        Game.World.PlayerCharacter(StaticWorldData.World).Money -= repairCost
        item.Repair.Perform()
        Dim oldIndex = currentItemIndex
        Initialize()
        If Not items.Any Then
            Return UIState.InPlay
        End If
        currentItemIndex = oldIndex Mod items.Count
        Return UIState.ShoppeRepair
    End Function
End Class
