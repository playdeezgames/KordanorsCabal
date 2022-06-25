﻿Friend Class ConstableModeProcessor
    Inherits ModeProcessor

    Const WelcomeButtonIndex = 0
    Const GoodByeButtonIndex = 9


    Friend Overrides Sub UpdateBuffer(player As PlayerCharacter, buffer As PatternBuffer)
        ShowHeader(buffer, player.Location.Feature.Name)
        Select Case CurrentButtonIndex
            Case GoodByeButtonIndex
                buffer.WriteText((0, 1), "Fare well!", False, Hue.Black)
            Case Else
                buffer.WriteText((0, 1), "Brint it, trolls!", False, Hue.Black)
        End Select
    End Sub

    Friend Overrides Sub UpdateButtons(player As PlayerCharacter)
        Buttons(WelcomeButtonIndex).Title = "Hello!"
        Buttons(GoodByeButtonIndex).Title = "Good-bye"
    End Sub

    Friend Overrides Sub HandleButton(player As PlayerCharacter, button As Button)
        Select Case button.Index
            Case GoodByeButtonIndex
                PopButtonIndex()
                player.Mode = PlayerMode.Neutral
        End Select
    End Sub
End Class