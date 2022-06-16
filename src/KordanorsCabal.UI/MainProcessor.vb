Imports SPLORR.UI

Public Module MainProcessor
    ReadOnly processors As IReadOnlyDictionary(Of UIState, IProcessor) =
        New Dictionary(Of UIState, IProcessor) From
        {
            {UIState.TitleScreen, New TitleScreenProcessor}
        }
    Public Function ProcessCommand(uiState As UIState, command As Command) As UIState
        Return processors(uiState).ProcessCommand(command)
    End Function

    Public Sub UpdateBuffer(uiState As UIState, buffer As PatternBuffer)
        processors(uiState).UpdateBuffer(buffer)
    End Sub
End Module
