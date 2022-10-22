Imports KordanorsCabal.Data
Imports SPLORR.Data

Friend Class DeadProcessor
    Inherits BaseProcessor

    Public Overrides Sub UpdateBuffer(worldData As IWorldData, buffer As PatternBuffer)
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteText((0, 0), "yer dead!", False, Hue.Red)
        If Game.World.PlayerCharacter(StaticWorldData.WorldData).Equipment.CurrentEquipment(EquipSlot.FromId(StaticWorldData.WorldData, 5L)) IsNot Nothing Then
            buffer.WriteText((0, 2), "But at least you died with dignity!", False, Hue.Black)
        End If
    End Sub

    Public Overrides Sub Initialize()
    End Sub

    Public Overrides Function ProcessCommand(worldData As IWorldData, command As Command) As UIState
        Select Case command
            Case Command.Green, Command.Blue
                StaticWorldData.WorldData.Reset()
                Return UIState.TitleScreen
            Case Else
                Return UIState.Dead
        End Select
    End Function
End Class
