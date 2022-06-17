Friend Interface IProcessor
    Function ProcessCommand(command As Command) As UIState
    Sub UpdateBuffer(buffer As PatternBuffer)
End Interface
