Imports KordanorsCabal.Data

Friend Class SpellListProcessor
    Inherits BaseProcessor

    Private items As List(Of (Long, Long))
    Private currentItemIndex As Integer = 0
    Const ListStartRow = 2
    Const ListHiliteRow = 10
    Const ListEndRow = 21

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Spell List", True, Hue.Blue)
        Dim player = Game.World.PlayerCharacter(StaticWorldData.WorldData)
        For row = ListStartRow To ListEndRow
            Dim itemIndex = row - ListHiliteRow + currentItemIndex
            If itemIndex >= 0 AndAlso itemIndex < items.Count Then
                Dim spellType = Game.SpellType.FromId(StaticWorldData.WorldData, items(itemIndex).Item1)
                Dim rowHue = If(player.Spellbook.CanCastSpell(spellType), Hue.Black, Hue.Red)
                buffer.FillCells((0, row), (buffer.Columns, 1), Pattern.Space, itemIndex = currentItemIndex, rowHue)
                buffer.WriteTextCentered(row, $"{spellType.Name}(Lvl{items(itemIndex).Item2})", itemIndex = currentItemIndex, rowHue)
            End If
        Next

        buffer.WriteTextCentered(buffer.Rows - 1, "Arrows, Space, Esc", False, Hue.Black)
    End Sub

    Public Overrides Sub Initialize()

        currentItemIndex = 0
        items = Game.World.PlayerCharacter(StaticWorldData.WorldData).Spellbook.Spells.Select(Function(x) (x.Key, x.Value)).ToList
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
                Return CastSpell()
        End Select
        Return UIState.SpellList
    End Function

    Private Function CastSpell() As UIState
        Game.World.PlayerCharacter(StaticWorldData.WorldData).Spellbook.Cast(Game.SpellType.FromId(StaticWorldData.WorldData, items(currentItemIndex).Item1))
        PushUIState(UIState.SpellList)
        Return UIState.Message
    End Function
End Class
