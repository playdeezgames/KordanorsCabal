Friend Class ConfirmQuitProcessor
    Implements IProcessor

    Const NoMenuItem = "No"
    Const YesMenuItem = "Yes"

    ReadOnly MenuItems As IReadOnlyList(Of String) =
        New List(Of String) From
        {
            NoMenuItem,
            YesMenuItem
        }
    Const MenuRow = 14
    Private currentItem As Integer = 0

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 10), "Are you sure you want to quit?", False, Hue.Red)
        Dim index As Integer = 0
        For Each menuItem In MenuItems
            buffer.WriteText(((buffer.Columns - menuItem.Length) \ 2, MenuRow + index), menuItem, index = currentItem, Hue.Orange)
            index += 1
        Next
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Up
                PreviousMenuItem()
            Case Command.Down
                NextMenuItem()
            Case Command.Green, Command.Blue
                Select Case MenuItems(currentItem)
                    Case YesMenuItem
                        Return UIState.None
                    Case NoMenuItem
                        Return UIState.TitleScreen
                End Select
        End Select
        Return UIState.ConfirmQuit
    End Function

    Private Sub NextMenuItem()
        currentItem = (currentItem + 1) Mod MenuItems.Count
    End Sub

    Private Sub PreviousMenuItem()
        currentItem = (currentItem + MenuItems.Count - 1) Mod MenuItems.Count
    End Sub
End Class
