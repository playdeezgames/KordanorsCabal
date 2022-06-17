Imports SPLORR.UI

Public Module MainProcessor
    ReadOnly processors As IReadOnlyDictionary(Of UIState, IProcessor) =
        New Dictionary(Of UIState, IProcessor) From
        {
            {UIState.AboutScreen, New AboutScreenProcessor},
            {UIState.ConfirmQuit, New ConfirmQuitProcessor},
            {UIState.InstructionsScreen, New InstructionsScreenProcessor},
            {UIState.OptionsScreen, New OptionsScreenProcessor},
            {UIState.ScreenSizer, New ScreenSizerProcessor},
            {UIState.TitleScreen, New TitleScreenProcessor}
        }
    Public Function ProcessCommand(uiState As UIState, command As Command) As UIState
        Dim newState = processors(uiState).ProcessCommand(command)
        If newState <> UIState.None AndAlso newState <> uiState Then
            processors(newState).Initialize()
        End If
        Return newState
    End Function

    Public Sub UpdateBuffer(uiState As UIState, buffer As PatternBuffer)
        If processors.ContainsKey(uiState) Then
            processors(uiState).UpdateBuffer(buffer)
        End If
    End Sub
End Module
