﻿Imports KordanorsCabal.Data

Friend Class ShoppeRepairProcessor
    Inherits ShoppeProcessor(Of IItem)

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, $"Repair", True, Hue.Blue)
        buffer.WriteTextCentered(1, $"Money: {World.FromWorldData(worldData).PlayerCharacter.Statuses.Money}", False, Hue.Black)

        For row = ListStartRow + 1 To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, Hue.Black)
                Dim text = $"{items(itemIndex).Name}({items(itemIndex).Repair.RepairCost(World.FromWorldData(worldData).PlayerCharacter.ShoppeType)})"
                buffer.WriteTextCentered(row, text, itemIndex = currentItemIndex, Hue.Black)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Overrides Sub Initialize()

        currentItemIndex = 0
        Dim repairs = World.FromWorldData(WorldData).PlayerCharacter.ShoppeType.Repairs
        items = World.FromWorldData(WorldData).PlayerCharacter.
            Repair.ItemsToRepair(World.FromWorldData(WorldData).PlayerCharacter.ShoppeType).
            Where(Function(x) World.FromWorldData(WorldData).PlayerCharacter.ShoppeType.WillRepair(x.ItemType)).ToList
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
                Return RepairItem()
        End Select
        Return UIState.ShoppeRepair
    End Function

    Private Function RepairItem() As UIState
        If Not items.Any Then
            Return UIState.InPlay
        End If
        Dim item = items(currentItemIndex)
        Dim repairCost = item.Repair.RepairCost(Game.ShoppeType.FromId(WorldData, 2))
        If World.FromWorldData(WorldData).PlayerCharacter.Statuses.Money < repairCost Then
            Return UIState.ShoppeRepair
        End If
        World.FromWorldData(WorldData).PlayerCharacter.Statuses.Money -= repairCost
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
