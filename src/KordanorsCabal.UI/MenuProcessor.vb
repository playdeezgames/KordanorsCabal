﻿Public MustInherit Class MenuProcessor
    Implements IProcessor

    Private ReadOnly MenuItems As IReadOnlyList(Of (String, Func(Of UIState)))
    Private ReadOnly MenuRow As Integer
    Private currentItem As Integer = 0
    Private ReadOnly uiState As UIState

    Sub New(menuItems As IReadOnlyList(Of (String, Func(Of UIState))), menuRow As Integer, uiState As UIState)
        Me.MenuItems = menuItems
        Me.MenuRow = menuRow
        Me.uiState = uiState
    End Sub

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        ShowPrompt(buffer)
        Dim index As Integer = 0
        For Each menuItem In MenuItems
            buffer.WriteText(((buffer.Columns - menuItem.Item1.Length) \ 2, MenuRow + index), menuItem.Item1, index = currentItem, Hue.Orange)
            index += 1
        Next
    End Sub

    Protected MustOverride Sub ShowPrompt(buffer As PatternBuffer)

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Up
                PreviousMenuItem()
            Case Command.Down
                NextMenuItem()
            Case Command.Green, Command.Blue
                Return MenuItems(currentItem).Item2.Invoke()
        End Select
        Return uiState
    End Function
    Private Sub NextMenuItem()
        currentItem = (currentItem + 1) Mod MenuItems.Count
    End Sub

    Private Sub PreviousMenuItem()
        currentItem = (currentItem + MenuItems.Count - 1) Mod MenuItems.Count
    End Sub
End Class
