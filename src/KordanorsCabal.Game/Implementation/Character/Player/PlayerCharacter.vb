Public Class PlayerCharacter
    Inherits Character
    Shared ReadOnly Property Messages As New Queue(Of Message)
    Public Overrides Sub EnqueueMessage(ParamArray lines() As String)
        EnqueueMessage(Nothing, lines)
    End Sub
    Public Overrides Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String)
        If sfx.HasValue Then
            Messages.Enqueue(Message.Create(sfx.Value, lines))
            Return
        End If
        Messages.Enqueue(Message.Create(lines))
    End Sub
    Sub New(worldData As IWorldData)
        MyBase.New(worldData, worldData.Player.Read().Value)
    End Sub
End Class
