﻿Imports KordanorsCabal.Data

Friend Class ShoppeBuyProcessor
    Inherits ShoppeProcessor(Of (IItemType, Long))

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, $"Buy", True, Hue.Blue)
        buffer.WriteTextCentered(1, $"Money: {World.FromWorldData(worldData).PlayerCharacter.Statuses.Money}", False, Hue.Black)

        For row = ListStartRow + 1 To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                buffer.WriteTextCentered(row, $"{items(itemIndex).Item1.Name}({items(itemIndex).Item2})", itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Overrides Sub Initialize()

        currentItemIndex = 0
        Dim money = World.FromWorldData(WorldData).PlayerCharacter.Statuses.Money
        items = World.FromWorldData(WorldData).PlayerCharacter.ShoppeType.Prices.Where(Function(x) x.Value <= money).Select(Function(x) (x.Key, x.Value)).ToList
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
        Select Case command
            Case Command.Red
                World.FromWorldData(worldData).PlayerCharacter.ShoppeType = Nothing
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
                Return BuyItem()
        End Select
        Return UIState.ShoppeBuy
    End Function

    Private Function BuyItem() As UIState
        If Not items.Any Then
            Return UIState.InPlay
        End If
        Dim itemType = items(currentItemIndex)
        World.FromWorldData(WorldData).PlayerCharacter.Statuses.Money -= itemType.Item2
        World.FromWorldData(WorldData).PlayerCharacter.Items.Inventory.Add(Game.Item.Create(WorldData, Game.ItemType.FromId(WorldData, itemType.Item1.Id)))
        Dim oldIndex = currentItemIndex
        Initialize()
        If Not items.Any Then
            Return UIState.InPlay
        End If
        currentItemIndex = oldIndex Mod items.Count
        Return UIState.ShoppeBuy
    End Function
End Class
