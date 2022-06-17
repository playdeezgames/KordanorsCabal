Friend Class FinalizeCharacterProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        Throw New NotImplementedException()
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
        Throw New NotImplementedException()
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Throw New NotImplementedException()
    End Function
End Class
