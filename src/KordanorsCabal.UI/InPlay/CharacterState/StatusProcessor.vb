Imports KordanorsCabal.Data

Friend Class StatusProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)

        buffer.WriteText((0, 1), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Strength).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Strength))}", False, Hue.Black)
        buffer.WriteText((0, 2), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Dexterity).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Dexterity))}", False, Hue.Black)
        buffer.WriteText((0, 3), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.HP).Abbreviation} {player.Health.Current}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 6L))}", False, Hue.Black)

        buffer.WriteText((11, 1), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Influence).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Influence))}", False, Hue.Black)
        buffer.WriteText((11, 2), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Willpower).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Willpower))}", False, Hue.Black)
        buffer.WriteText((11, 3), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.MP).Abbreviation} {player.MentalCombat.CurrentMP}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.MP))}", False, Hue.Black)

        buffer.WriteText((0, 5), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Power).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Power))}", False, Hue.Black)
        buffer.WriteText((0, 6), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Mana).Abbreviation} {player.Mana.CurrentMana}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Mana))}", False, Hue.Black)

        buffer.WriteText((11, 5), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.XP).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 16L))}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 17L))}", False, Hue.Black)

        buffer.WriteText((0, 8), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Money).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 14L))}", False, Hue.Black)
        buffer.WriteText((0, 10), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Hunger).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 20L))}", False, Hue.Black)
        Dim row = 11
        If player.Highness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Highness).Abbreviation} {player.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Drunkenness).Abbreviation} {player.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.FoodPoisoning).Abbreviation} {player.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Chafing > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.StatisticTypes.Chafing).Abbreviation} {player.Chafing}", False, Hue.Black)
            row += 1
        End If
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Blue, Command.Green, Command.Red
                Return UIState.InPlay
            Case Else
                Return UIState.Status
        End Select
    End Function
End Class
