Imports KordanorsCabal.Data

Friend Class InPlayProcessor
    Implements IProcessor

    Private ReadOnly _modeProcessors As IReadOnlyDictionary(Of Long, ModeProcessor) =
        New Dictionary(Of Long, ModeProcessor) From
        {
            {Game.Constants.PlayerModes.BlackMage, New BlackMageModeProcessor},
            {Game.Constants.PlayerModes.BlackMarket, New BlackMarketModeProcessor},
            {Game.Constants.PlayerModes.Blacksmith, New BlacksmithModeProcessor},
            {Game.Constants.PlayerModes.Constable, New ConstableModeProcessor},
            {Game.Constants.PlayerModes.Chicken, New ChickenModeProcessor},
            {Game.Constants.PlayerModes.Elder, New ElderModeProcessor},
            {Game.Constants.PlayerModes.Healer, New HealerModeProcessor},
            {Game.Constants.PlayerModes.InnKeeper, New InnKeeperModeProcessor},
            {Game.Constants.PlayerModes.Move, New MoveModeProcessor},
            {Game.Constants.PlayerModes.Neutral, New NeutralModeProcessor},
            {Game.Constants.PlayerModes.TownDrunk, New TownDrunkModeProcessor},
            {Game.Constants.PlayerModes.Turn, New TurnModeProcessor}
        }


    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        Dim modeProcessor = _modeProcessors(Game.World.PlayerCharacter(StaticWorldData.World).Mode)
        Dim player = Game.World.PlayerCharacter(StaticWorldData.World)
        modeProcessor.UpdateBuffer(player, buffer)
        For Each button In ModeProcessor.Buttons
            button.Clear()
        Next
        modeProcessor.UpdateButtons(player)
        DrawButtons(buffer)
    End Sub

    Private Sub DrawButtons(buffer As PatternBuffer)
        For Each button In ModeProcessor.Buttons
            button.Draw(buffer, ModeProcessor.CurrentButtonIndex)
        Next
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Dim nextState = UIState.InPlay
        Select Case command
            Case Command.Green, Command.Blue
                nextState = _modeProcessors(Game.World.PlayerCharacter(StaticWorldData.World).Mode).HandleButton(Game.World.PlayerCharacter(StaticWorldData.World), ModeProcessor.Buttons(ModeProcessor.CurrentButtonIndex))
            Case Command.Down
                ModeProcessor.CurrentButtonIndex = (ModeProcessor.CurrentButtonIndex + 1) Mod ModeProcessor.Buttons.Count
            Case Command.Up
                ModeProcessor.CurrentButtonIndex = (ModeProcessor.CurrentButtonIndex + ModeProcessor.Buttons.Count - 1) Mod ModeProcessor.Buttons.Count
            Case Command.Left, Command.Right
                ModeProcessor.CurrentButtonIndex = (ModeProcessor.CurrentButtonIndex + ModeProcessor.Buttons.Count \ 2) Mod ModeProcessor.Buttons.Count
            Case Command.Red
                nextState = _modeProcessors(Game.World.PlayerCharacter(StaticWorldData.World).Mode).HandleRed(Game.World.PlayerCharacter(StaticWorldData.World))
        End Select
        Return nextState
    End Function
End Class
