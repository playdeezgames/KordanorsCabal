Public Class Message
    ReadOnly Property Lines As IEnumerable(Of String)
    Property Sfx As Sfx?
    Sub New(lines As IEnumerable(Of String), Optional sfx As Sfx? = Nothing)
        Me.Lines = lines
        Me.Sfx = sfx
    End Sub
    Shared Function Create(sfx As Sfx, ParamArray lines() As String) As Message
        Return New Message(lines, sfx)
    End Function
    Shared Function Create(ParamArray lines() As String) As Message
        Return New Message(lines, Nothing)
    End Function
End Class
