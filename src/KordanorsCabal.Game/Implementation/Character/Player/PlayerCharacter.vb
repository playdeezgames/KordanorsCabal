Public Class PlayerCharacter
    Inherits Character
    Implements IPlayerCharacter
    Shared ReadOnly Property Messages As New Queue(Of Message)
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
    Public Property Mode As Long Implements IPlayerCharacter.Mode
        Get
            Return WorldData.Player.ReadPlayerMode().Value
        End Get
        Set(value As Long)
            WorldData.Player.WritePlayerMode(value)
        End Set
    End Property
End Class
