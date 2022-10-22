Public MustInherit Class BaseProcessor
    Implements IProcessor
    Public MustOverride Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
    Public MustOverride Sub Initialize() Implements IProcessor.Initialize
    Public MustOverride Function ProcessCommand(worldData As IWorldData, command As Command) As UIState Implements IProcessor.ProcessCommand
End Class
