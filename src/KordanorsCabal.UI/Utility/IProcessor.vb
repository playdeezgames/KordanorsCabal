Friend Interface IProcessor
    Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
    Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
    Sub Initialize()
End Interface
