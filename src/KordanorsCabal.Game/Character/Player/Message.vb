Public Class Message
    ReadOnly Property Lines As IEnumerable(Of String)
    Property Sfx As Sfx?
    Sub New(lines As IEnumerable(Of String), Optional sfx As Sfx? = Nothing)
        Me.Lines = lines
        Me.Sfx = sfx
    End Sub
End Class
