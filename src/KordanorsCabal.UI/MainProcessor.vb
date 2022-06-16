Imports SPLORR.UI

Public Module MainProcessor
    Public Function ProcessCommand(uiState As UIState, command As Command) As UIState
        Return uiState
    End Function

    Public Sub UpdateBuffer(uiState As UIState, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 0), "OHAI!", False, Hue.Blue)
    End Sub
End Module
