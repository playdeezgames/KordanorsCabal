Imports KordanorsCabal.Data
Imports SPLORR.Data

Friend Class DeadProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 0), "yer dead!", False, Hue.Red)
        If Game.World.PlayerCharacter(StaticWorldData.World).CurrentEquipment(EquipSlot.FromId(StaticWorldData.World, 5L)) IsNot Nothing Then
            buffer.WriteText((0, 2), "But at least you died with dignity!", False, Hue.Black)
        End If
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        Select Case command
            Case Command.Green, Command.Blue
                StaticWorldData.World.Reset()
                Return UIState.TitleScreen
            Case Else
                Return UIState.Dead
        End Select
    End Function
End Class
