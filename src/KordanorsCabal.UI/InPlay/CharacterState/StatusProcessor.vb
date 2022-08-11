Friend Class StatusProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = World.PlayerCharacter

        buffer.WriteText((0, 1), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Strength).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Strength))}", False, Hue.Black)
        buffer.WriteText((0, 2), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Dexterity).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Dexterity))}", False, Hue.Black)
        buffer.WriteText((0, 3), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.HP).Abbreviation} {player.CurrentHP}/{player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.HP))}", False, Hue.Black)

        buffer.WriteText((11, 1), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Influence).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Influence))}", False, Hue.Black)
        buffer.WriteText((11, 2), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Willpower).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Willpower))}", False, Hue.Black)
        buffer.WriteText((11, 3), $"{CharacterStatisticType.FromName(MP).Abbreviation} {player.CurrentMP}/{player.GetStatistic(CharacterStatisticType.FromName(MP))}", False, Hue.Black)

        buffer.WriteText((0, 5), $"{CharacterStatisticType.FromName(Power).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(Power))}", False, Hue.Black)
        buffer.WriteText((0, 6), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Mana).Abbreviation} {player.CurrentMana}/{player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Mana))}", False, Hue.Black)

        buffer.WriteText((11, 5), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XP).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XP))}/{player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XPGoal))}", False, Hue.Black)

        buffer.WriteText((0, 8), $"{CharacterStatisticType.FromName(Money).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(Money))}", False, Hue.Black)
        buffer.WriteText((0, 10), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Hunger).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Hunger))}", False, Hue.Black)
        Dim row = 11
        If player.Highness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Highness).Abbreviation} {player.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Drunkenness).Abbreviation} {player.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.FoodPoisoning).Abbreviation} {player.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Chafing > 0 Then
            buffer.WriteText((0, row), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Chafing).Abbreviation} {player.Chafing}", False, Hue.Black)
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
