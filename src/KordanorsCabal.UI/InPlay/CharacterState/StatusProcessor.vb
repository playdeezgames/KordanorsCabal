Friend Class StatusProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = World.PlayerCharacter

        buffer.WriteText((0, 1), $"{OldCharacterStatisticType.Strength.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Strength)}", False, Hue.Black)
        buffer.WriteText((0, 2), $"{OldCharacterStatisticType.Dexterity.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Dexterity)}", False, Hue.Black)
        buffer.WriteText((0, 3), $"{OldCharacterStatisticType.HP.Abbreviation} {player.CurrentHP}/{player.GetStatistic(OldCharacterStatisticType.HP)}", False, Hue.Black)

        buffer.WriteText((11, 1), $"{OldCharacterStatisticType.Influence.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Influence)}", False, Hue.Black)
        buffer.WriteText((11, 2), $"{OldCharacterStatisticType.Willpower.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Willpower)}", False, Hue.Black)
        buffer.WriteText((11, 3), $"{OldCharacterStatisticType.MP.Abbreviation} {player.CurrentMP}/{player.GetStatistic(OldCharacterStatisticType.MP)}", False, Hue.Black)

        buffer.WriteText((0, 5), $"{OldCharacterStatisticType.Power.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Power)}", False, Hue.Black)
        buffer.WriteText((0, 6), $"{OldCharacterStatisticType.Mana.Abbreviation} {player.CurrentMana}/{player.GetStatistic(OldCharacterStatisticType.Mana)}", False, Hue.Black)

        buffer.WriteText((11, 5), $"{OldCharacterStatisticType.XP.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.XP)}/{player.GetStatistic(OldCharacterStatisticType.XPGoal)}", False, Hue.Black)

        buffer.WriteText((0, 8), $"{OldCharacterStatisticType.Money.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Money)}", False, Hue.Black)
        buffer.WriteText((0, 10), $"{OldCharacterStatisticType.Hunger.Abbreviation} {player.GetStatistic(OldCharacterStatisticType.Hunger)}", False, Hue.Black)
        Dim row = 11
        If player.Highness > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.Highness.Abbreviation} {player.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.Drunkenness.Abbreviation} {player.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.FoodPoisoning.Abbreviation} {player.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Chafing > 0 Then
            buffer.WriteText((0, row), $"{OldCharacterStatisticType.Chafing.Abbreviation} {player.Chafing}", False, Hue.Black)
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
