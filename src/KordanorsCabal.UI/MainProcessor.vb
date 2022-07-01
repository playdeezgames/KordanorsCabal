Imports SPLORR.UI

Public Module MainProcessor
    ReadOnly processors As IReadOnlyDictionary(Of UIState, IProcessor) =
        New Dictionary(Of UIState, IProcessor) From
        {
            {UIState.AboutScreen, New AboutScreenProcessor},
            {UIState.ConfirmAbandonGame, New ConfirmAbandonGameProcessor},
            {UIState.ConfirmQuit, New ConfirmQuitProcessor},
            {UIState.Dead, New DeadProcessor},
            {UIState.FinalizeCharacter, New FinalizeCharacterProcessor},
            {UIState.GameMenuScreen, New GameMenuScreenProcessor},
            {UIState.GroundInventory, New GroundInventoryProcessor},
            {UIState.InPlay, New InPlayProcessor},
            {UIState.InstructionsScreen, New InstructionsScreenProcessor},
            {UIState.InteractItem, New InteractItemProcessor},
            {UIState.Inventory, New InventoryProcessor},
            {UIState.LoadGameScreen, New LoadGameScreenProcessor},
            {UIState.Map, New MapProcessor},
            {UIState.Message, New MessageProcessor},
            {UIState.MuxVolumizer, New MuxVolumizerProcessor},
            {UIState.OptionsScreen, New OptionsScreenProcessor},
            {UIState.Prolog, New PrologProcessor},
            {UIState.SaveGameScreen, New SaveGameScreenProcessor},
            {UIState.ScreenSizer, New ScreenSizerProcessor},
            {UIState.SfxVolumizer, New SfxVolumizerProcessor},
            {UIState.Status, New StatusProcessor},
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

    Public PushUIState As Action(Of UIState)
    Public PopUIState As Func(Of UIState)

End Module
