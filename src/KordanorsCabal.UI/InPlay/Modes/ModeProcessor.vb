Public MustInherit Class ModeProcessor
    Friend MustOverride Sub UpdateBuffer(buffer As PatternBuffer)
    Friend MustOverride Sub UpdateButtons()
    Friend MustOverride Sub HandleButton(button As Button)
    Friend Shared Buttons As IReadOnlyList(Of Button) =
        New List(Of Button) From
        {
            New Button(0, "0", (0, 18), 11),
            New Button(1, "1", (0, 19), 11),
            New Button(2, "2", (0, 20), 11),
            New Button(3, "3", (0, 21), 11),
            New Button(4, "4", (0, 22), 11),
            New Button(5, "5", (11, 18), 11),
            New Button(6, "6", (11, 19), 11),
            New Button(7, "7", (11, 20), 11),
            New Button(8, "8", (11, 21), 11),
            New Button(9, "9", (11, 22), 11)
        }
    Private Shared currentButtonStack As New Stack(Of Integer)
    Friend Shared Property CurrentButtonIndex As Integer = 0
    Friend Shared Sub PushButtonIndex()
        currentButtonStack.Push(CurrentButtonIndex)
    End Sub
    Friend Shared Sub PopButtonIndex()
        If currentButtonStack.Any() Then
            CurrentButtonIndex = currentButtonStack.Pop()
            Return
        End If
        CurrentButtonIndex = 0
    End Sub
End Class
