Friend Class StatusProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Status", True, Hue.Blue)
        Dim player = World.PlayerCharacter

        buffer.WriteText((0, 1), $"STR {player.GetStatistic(CharacterStatisticType.Strength)}", False, Hue.Black)
        buffer.WriteText((0, 2), $"DEX {player.GetStatistic(CharacterStatisticType.Dexterity)}", False, Hue.Black)
        buffer.WriteText((0, 3), $"HP {player.CurrentHP}/{player.GetStatistic(CharacterStatisticType.HP)}", False, Hue.Black)

        buffer.WriteText((11, 1), $"INF {player.GetStatistic(CharacterStatisticType.Influence)}", False, Hue.Black)
        buffer.WriteText((11, 2), $"WIL {player.GetStatistic(CharacterStatisticType.Willpower)}", False, Hue.Black)
        buffer.WriteText((11, 3), $"MP {player.CurrentMP}/{player.GetStatistic(CharacterStatisticType.MP)}", False, Hue.Black)

        buffer.WriteText((0, 5), $"POW {player.GetStatistic(CharacterStatisticType.Power)}", False, Hue.Black)
        buffer.WriteText((0, 6), $"Mana {player.CurrentMana}/{player.GetStatistic(CharacterStatisticType.Mana)}", False, Hue.Black)

        buffer.WriteText((11, 5), $"XP {player.GetStatistic(CharacterStatisticType.XP)}/{player.GetStatistic(CharacterStatisticType.XPGoal)}", False, Hue.Black)

        buffer.WriteText((0, 8), $"Money {player.GetStatistic(CharacterStatisticType.Money)}", False, Hue.Black)
        buffer.WriteText((0, 10), $"Hunger {player.GetStatistic(CharacterStatisticType.Hunger)}", False, Hue.Black)
        Dim row = 11
        If player.Highness > 0 Then
            buffer.WriteText((0, row), $"Highness {player.Highness}", False, Hue.Black)
            row += 1
        End If
        If player.Drunkenness > 0 Then
            buffer.WriteText((0, row), $"Drunkenness {player.Drunkenness}", False, Hue.Black)
            row += 1
        End If
        If player.FoodPoisoning > 0 Then
            buffer.WriteText((0, row), $"Food Poisoning {player.FoodPoisoning}", False, Hue.Black)
            row += 1
        End If
        If player.Chafing > 0 Then
            buffer.WriteText((0, row), $"Chafing {player.Chafing}", False, Hue.Black)
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
