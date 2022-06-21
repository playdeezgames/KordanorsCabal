Public Class Button
    Property Title As String
    Property Position As (Integer, Integer)
    Property Size As Integer
    Property Index As Integer
    Sub New(index As Integer, title As String, position As (Integer, Integer), size As Integer)
        Me.Index = index
        Me.Title = title
        Me.Position = position
        Me.Size = size
    End Sub

    Friend Sub Draw(buffer As PatternBuffer, currentButton As Integer)
        Dim inverted = (currentButton = Index)
        Dim drawHue = Hue.Orange
        buffer.FillCells(Position, (Size, 1), Pattern.Space, inverted, drawHue)
        buffer.WriteText(Position, Title, inverted, drawHue)
    End Sub

    Friend Sub Clear()
        Title = ""
    End Sub
End Class
