Friend Class ShoppeSellProcessor
    Inherits ShoppeProcessor

    Public Overrides Sub UpdateBuffer(buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.FillCells((0, 0), (buffer.Columns, 1), Pattern.Space, True, Hue.Blue)
        buffer.WriteTextCentered(0, "Sell", True, Hue.Blue)
    End Sub

    Public Overrides Sub Initialize()
    End Sub

    Public Overrides Function ProcessCommand(command As Command) As UIState
        Return UIState.InPlay
    End Function
End Class
