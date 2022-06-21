Public MustInherit Class ModeProcessor
    Friend MustOverride Sub UpdateBuffer(buffer As PatternBuffer)
    Friend MustOverride Sub UpdateButtons(buttons As IReadOnlyList(Of Button))
    Friend MustOverride Sub HandleButton(button As Button)
End Class
