Imports KordanorsCabal.Data

Friend Class StatusProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)

        buffer.WriteText((0, 1), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType1).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType1))}", False, Hue.Black)
        buffer.WriteText((0, 2), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType2).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType2))}", False, Hue.Black)
        buffer.WriteText((0, 3), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType6).Abbreviation} {player.Health.Current}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 6L))}", False, Hue.Black)

        buffer.WriteText((11, 1), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType3).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType3))}", False, Hue.Black)
        buffer.WriteText((11, 2), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType4).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType4))}", False, Hue.Black)
        buffer.WriteText((11, 3), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType7).Abbreviation} {player.MentalCombat.CurrentMP}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType7))}", False, Hue.Black)

        buffer.WriteText((0, 5), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType5).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType5))}", False, Hue.Black)
        buffer.WriteText((0, 6), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType8).Abbreviation} {player.Mana.CurrentMana}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType8))}", False, Hue.Black)

        buffer.WriteText((11, 5), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType16).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType16))}/{player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType17))}", False, Hue.Black)

        buffer.WriteText((0, 8), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType14).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType14))}", False, Hue.Black)
        buffer.WriteText((0, 10), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType20).Abbreviation} {player.Statistics.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType20))}", False, Hue.Black)
        Dim row = 11
        If player.Statuses.Highness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType19).Abbreviation} {player.Statuses.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Statuses.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType18).Abbreviation} {player.Statuses.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.Statuses.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType21).Abbreviation} {player.Statuses.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Statuses.Chafing > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, CharacterStatisticType22).Abbreviation} {player.Statuses.Chafing}", False, Hue.Black)
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
