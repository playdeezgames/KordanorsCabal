Public Class MuxVolumizerProcessor
    Inherits MenuProcessor
    Private Shared Function MakeMenuItem(percent As Integer) As (String, Func(Of UIState))
        Return ($"{percent}%", Function()
                                   SetCurrentMuxVolume.Invoke(CSng(percent * 0.01))
                                   Return UIState.OptionsScreen
                               End Function)
    End Function
    Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                MakeMenuItem(0),
                MakeMenuItem(10),
                MakeMenuItem(20),
                MakeMenuItem(30),
                MakeMenuItem(40),
                MakeMenuItem(50),
                MakeMenuItem(60),
                MakeMenuItem(70),
                MakeMenuItem(80),
                MakeMenuItem(90),
                MakeMenuItem(100)
            },
            6,
            UIState.MuxVolumizer)
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(4, "MUX Volume", False, Hue.Blue)
    End Sub

    Public Overrides Sub Initialize()
        currentItem = CInt(GetCurrentMuxVolume.Invoke() * 10.0)
    End Sub

    Public Shared Property GetCurrentMuxVolume As Func(Of Single)
    Public Shared Property SetCurrentMuxVolume As Action(Of Single)
    Public Overrides Function HandleRed() As UIState
        Return UIState.OptionsScreen
    End Function
End Class
