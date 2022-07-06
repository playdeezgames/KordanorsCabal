MustInherit Class ShoppeProcessor
    Implements IProcessor

    Public Shared Property ShoppeType As ShoppeType
    Public MustOverride Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
    Public MustOverride Sub Initialize() Implements IProcessor.Initialize
    Public MustOverride Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
End Class
