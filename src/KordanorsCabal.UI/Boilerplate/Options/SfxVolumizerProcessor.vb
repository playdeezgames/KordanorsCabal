Public Class SfxVolumizerProcessor
    Inherits MenuProcessor

    Sub New()
        MyBase.New(
            New List(Of (String, Func(Of UIState))) From
            {
                ("0%", Function()
                           SetCurrentSfxVolume.Invoke(0)
                           Return UIState.OptionsScreen
                       End Function),
                ("10%", Function()
                            SetCurrentSfxVolume.Invoke(0.1)
                            Return UIState.OptionsScreen
                        End Function),
                ("20%", Function()
                            SetCurrentSfxVolume.Invoke(0.2)
                            Return UIState.OptionsScreen
                        End Function),
                ("30%", Function()
                            SetCurrentSfxVolume.Invoke(0.3)
                            Return UIState.OptionsScreen
                        End Function),
                ("40%", Function()
                            SetCurrentSfxVolume.Invoke(0.4)
                            Return UIState.OptionsScreen
                        End Function),
                ("50%", Function()
                            SetCurrentSfxVolume.Invoke(0.5)
                            Return UIState.OptionsScreen
                        End Function),
                ("60%", Function()
                            SetCurrentSfxVolume.Invoke(0.6)
                            Return UIState.OptionsScreen
                        End Function),
                ("70%", Function()
                            SetCurrentSfxVolume.Invoke(0.7)
                            Return UIState.OptionsScreen
                        End Function),
                ("80%", Function()
                            SetCurrentSfxVolume.Invoke(0.8)
                            Return UIState.OptionsScreen
                        End Function),
                ("90%", Function()
                            SetCurrentSfxVolume.Invoke(0.9)
                            Return UIState.OptionsScreen
                        End Function),
                ("100%", Function()
                             SetCurrentSfxVolume.Invoke(1)
                             Return UIState.OptionsScreen
                         End Function)
            },
            6,
            UIState.SfxVolumizer)
    End Sub

    Protected Overrides Sub ShowPrompt(buffer As PatternBuffer)
        buffer.WriteTextCentered(4, "SFX Volume", False, Hue.Blue)
    End Sub

    Public Overrides Sub Initialize()
        currentItem = CInt(GetCurrentSfxVolume.Invoke() * 10.0)
    End Sub

    Public Shared Property GetCurrentSfxVolume As Func(Of Single)
    Public Shared Property SetCurrentSfxVolume As Action(Of Single)
    Public Overrides Function HandleRed() As UIState
        Return UIState.OptionsScreen
    End Function
End Class
