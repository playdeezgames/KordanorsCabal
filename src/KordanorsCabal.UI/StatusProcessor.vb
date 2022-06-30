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

        buffer.WriteText((0, 8), $"Money {player.GetStatistic(CharacterStatisticType.Money)}", False, Hue.Black)
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
