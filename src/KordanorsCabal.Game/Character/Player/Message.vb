Public Class Message
    ReadOnly Property Lines As IEnumerable(Of String)
    Sub New(lines As IEnumerable(Of String))
        Me.Lines = lines
    End Sub
End Class
