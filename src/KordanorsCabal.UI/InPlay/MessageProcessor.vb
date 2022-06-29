﻿Friend Class MessageProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        Dim message = PlayerCharacter.Messages.First
        Dim row = 0
        buffer.Fill(Pattern.Space, False, Hue.Black)
        For Each line In message.Lines
            buffer.WriteText((0, row), line, False, Hue.Black)
            row += ((line.Length + buffer.Columns - 1) \ buffer.Columns)
        Next
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Green, Command.Blue
                Dim messages = PlayerCharacter.Messages
                messages.Dequeue()
                If messages.Any Then
                    Return UIState.Message
                End If
                Return UIState.InPlay
        End Select
        Return UIState.Message
    End Function
End Class
