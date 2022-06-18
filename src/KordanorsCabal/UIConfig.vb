Imports System.IO
Imports System.Text.Json

Public Class UIConfig
    Property ScreenSize As Integer
    Property SfxVolume As Single
    Property MuxVolume As Single
    Const DefaultScreenSize As Integer = 4
    Const DefaultSfxVolume As Single = 0.5
    Const DefaultMuxVolume As Single = 0.5
    Const FileName As String = "config.json"
    Shared Function Load() As UIConfig
        Try
            Return JsonSerializer.Deserialize(Of UIConfig)(File.ReadAllText(fileName))
        Catch ex As Exception
            Return New UIConfig With
                {
                    .ScreenSize = DefaultScreenSize,
                    .SfxVolume = DefaultSfxVolume,
                    .MuxVolume = DefaultMuxVolume
                }
        End Try
    End Function
    Shared Sub Save(config As UIConfig)
        File.WriteAllText(FileName, JsonSerializer.Serialize(config))
    End Sub
End Class
