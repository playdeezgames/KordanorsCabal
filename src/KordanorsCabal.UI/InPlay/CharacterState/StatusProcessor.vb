Friend Class StatusProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = World.PlayerCharacter

        buffer.WriteText((0, 1), $"{OldCharacterStatisticType.Strength.ToNew.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Strength.ToNew)}", False, Hue.Black)
        buffer.WriteText((0, 2), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Dexterity).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Dexterity))}", False, Hue.Black)
        buffer.WriteText((0, 3), $"{OldCharacterStatisticType.HP.ToNew.Abbreviation} {player.CurrentHP}/{player.GetStatistic(OldCharacterStatisticType.HP.ToNew)}", False, Hue.Black)

        buffer.WriteText((11, 1), $"{CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Influence).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Influence))}", False, Hue.Black)
        buffer.WriteText((11, 2), $"{OldCharacterStatisticType.Willpower.ToNew.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Willpower.ToNew)}", False, Hue.Black)
        buffer.WriteText((11, 3), $"{CharacterStatisticType.FromName(MP).Abbreviation} {player.CurrentMP}/{player.GetStatistic(CharacterStatisticType.FromName(MP))}", False, Hue.Black)

        buffer.WriteText((0, 5), $"{CharacterStatisticType.FromName(Power).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(Power))}", False, Hue.Black)
        buffer.WriteText((0, 6), $"{OldCharacterStatisticType.Mana.ToNew.Abbreviation} {player.CurrentMana}/{player.GetStatistic(OldCharacterStatisticType.Mana.ToNew)}", False, Hue.Black)

        buffer.WriteText((11, 5), $"{OldCharacterStatisticType.XP.ToNew.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.XP.ToNew)}/{player.GetStatistic(OldCharacterStatisticType.XPGoal.ToNew)}", False, Hue.Black)

        buffer.WriteText((0, 8), $"{CharacterStatisticType.FromName(Money).Abbreviation} {player.GetStatistic(CharacterStatisticType.FromName(Money))}", False, Hue.Black)
        buffer.WriteText((0, 10), $"{OldCharacterStatisticType.Hunger.ToNew.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Hunger.ToNew)}", False, Hue.Black)
        Dim row = 11
        If player.Highness > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.Highness.ToNew.Abbreviation} {player.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.Drunkenness.ToNew.Abbreviation} {player.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.FoodPoisoning.ToNew.Abbreviation} {player.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Chafing > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.Chafing.ToNew.Abbreviation} {player.Chafing}", False, Hue.Black)
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
