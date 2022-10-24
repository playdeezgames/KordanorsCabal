﻿Imports KordanorsCabal.Data

Friend Class StatusProcessor
    Inherits BaseProcessor

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = World.FromWorldData(WorldData).PlayerCharacter

        buffer.WriteText((0, 1), $"{CharacterStatisticType.FromId(worldData, StatisticTypeStrength).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeStrength))}", False, Hue.Black)
        buffer.WriteText((0, 2), $"{CharacterStatisticType.FromId(worldData, StatisticTypeDexterity).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeDexterity))}", False, Hue.Black)
        buffer.WriteText((0, 3), $"{CharacterStatisticType.FromId(worldData, StatisticTypeHP).Abbreviation} {player.Health.Current}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, 6L))}", False, Hue.Black)

        buffer.WriteText((11, 1), $"{CharacterStatisticType.FromId(worldData, StatisticTypeInfluence).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeInfluence))}", False, Hue.Black)
        buffer.WriteText((11, 2), $"{CharacterStatisticType.FromId(worldData, StatisticTypeWillpower).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeWillpower))}", False, Hue.Black)
        buffer.WriteText((11, 3), $"{CharacterStatisticType.FromId(worldData, StatisticTypeMP).Abbreviation} {player.MentalCombat.CurrentMP}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeMP))}", False, Hue.Black)

        buffer.WriteText((0, 5), $"{CharacterStatisticType.FromId(worldData, StatisticTypePower).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypePower))}", False, Hue.Black)
        buffer.WriteText((0, 6), $"{CharacterStatisticType.FromId(worldData, StatisticTypeMana).Abbreviation} {player.Mana.CurrentMana}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeMana))}", False, Hue.Black)

        buffer.WriteText((11, 5), $"{CharacterStatisticType.FromId(worldData, StatisticTypeXP).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeXP))}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeXPGoal))}", False, Hue.Black)

        buffer.WriteText((0, 8), $"{CharacterStatisticType.FromId(worldData, StatisticTypeMoney).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeMoney))}", False, Hue.Black)
        buffer.WriteText((0, 10), $"{CharacterStatisticType.FromId(worldData, StatisticTypeHunger).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(worldData, StatisticTypeHunger))}", False, Hue.Black)
        Dim row = 11
        If player.Statuses.Highness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(worldData, StatisticTypeHighness).Abbreviation} {player.Statuses.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Statuses.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(worldData, StatisticTypeDrunkenness).Abbreviation} {player.Statuses.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.Statuses.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(worldData, StatisticTypeFoodPoisoning).Abbreviation} {player.Statuses.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Statuses.Chafing > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(worldData, StatisticTypeChafing).Abbreviation} {player.Statuses.Chafing}", False, Hue.Black)
            row += 1
        End If
    End Sub

    Public Overrides Sub Initialize()
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
        Select Case command
            Case Command.Blue, Command.Green, Command.Red
                Return UIState.InPlay
            Case Else
                Return UIState.Status
        End Select
    End Function
End Class
