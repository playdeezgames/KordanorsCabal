Imports KordanorsCabal.Data

Friend Class StatusProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)

        buffer.WriteText((0, 1), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType1).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType1))}", False, Hue.Black)
        buffer.WriteText((0, 2), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType2).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType2))}", False, Hue.Black)
        buffer.WriteText((0, 3), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType6).Abbreviation} {player.Health.Current}/{player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 6L))}", False, Hue.Black)

        buffer.WriteText((11, 1), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType3).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType3))}", False, Hue.Black)
        buffer.WriteText((11, 2), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType4).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType4))}", False, Hue.Black)
        buffer.WriteText((11, 3), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType7).Abbreviation} {player.CurrentMP}/{player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType7))}", False, Hue.Black)

        buffer.WriteText((0, 5), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType5).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType5))}", False, Hue.Black)
        buffer.WriteText((0, 6), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType8).Abbreviation} {player.CurrentMana}/{player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType8))}", False, Hue.Black)

        buffer.WriteText((11, 5), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType16).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 16L))}/{player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 17L))}", False, Hue.Black)

        buffer.WriteText((0, 8), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType14).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 14L))}", False, Hue.Black)
        buffer.WriteText((0, 10), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType20).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromId(StaticWorldData.World, 20L))}", False, Hue.Black)
        Dim row = 11
        If player.Highness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType19).Abbreviation} {player.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType18).Abbreviation} {player.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType21).Abbreviation} {player.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Chafing > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromId(StaticWorldData.World, Game.Constants.CharacterStatisticTypes.CharacterStatisticType22).Abbreviation} {player.Chafing}", False, Hue.Black)
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
