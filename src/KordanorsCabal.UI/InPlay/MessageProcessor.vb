Imports KordanorsCabal.Data

Friend Class MessageProcessor
    Inherits BaseProcessor

    Public Overrides Sub Initialize()
    End Sub

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        Dim message = PlayerCharacter.Messages.First
        If message.Sfx.HasValue Then
            SfxPlayer.Play(message.Sfx.Value)
            message.Sfx = Nothing
        End If
        Dim row = 0
        buffer.Fill(Pattern.Space, False, Hue.Black)
        For Each line In message.Lines
            buffer.WriteText((0, row), line, False, Hue.Black)
            row += ((line.Length + buffer.Columns - 1) \ buffer.Columns)
        Next
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
        Select Case command
            Case Command.Green, Command.Blue, Command.Red
                Dim messages = PlayerCharacter.Messages
                messages.Dequeue()
                If messages.Any Then
                    Return UIState.Message
                End If
                If Game.World.PlayerCharacter(StaticWorldData.WorldData).Health.IsDead Then
                    Return UIState.Dead
                End If
                Return PopUIState()
        End Select
        Return UIState.Message
    End Function
End Class
