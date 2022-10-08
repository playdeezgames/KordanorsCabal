MustInherit Class ShoppeProcessor(Of TListItem)
    Implements IProcessor

    Public Shared Property ShoppeType As OldShoppeType

    Protected items As List(Of TListItem)
    Protected currentItemIndex As Integer = 0
    Protected Const ListStartRow = 1
    Protected Const ListHiliteRow = 10
    Protected Const ListEndRow = 21

    Public MustOverride Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
    Public MustOverride Sub Initialize() Implements IProcessor.Initialize
    Public MustOverride Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
End Class
