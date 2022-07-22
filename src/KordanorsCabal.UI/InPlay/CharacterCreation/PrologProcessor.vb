Friend Class PrologProcessor
    Implements IProcessor

    Public Sub UpdateBuffer(buffer As PatternBuffer) Implements IProcessor.UpdateBuffer
        buffer.Fill(Pattern.Space, False, Hue.Blue)
        buffer.WriteTextCentered(0, "        Prolog        ", True, Hue.Blue)
        buffer.WriteLines((0, 2), New List(Of String) From
                          {
                            "The town of Zooperdan ",
                            "has fallen upon hard  ",
                            "times. A few years    ",
                            "ago, the Cabal of     ",
                            "Kordanor moved into a ",
                            "nearby abandoned      ",
                            "church. At first, no  ",
                            "one thought much of   ",
                            "the strange Cabal     ",
                            "members, but soon the ",
                            "villagers started     ",
                            "disappearing one by   ",
                            "one. So they pooled   ",
                            "their money and set   ",
                            "about hiring someone  ",
                            "to go in there and put",
                            "an end to the Cabal   ",
                            "once and for all! They",
                            "hired you. Good luck! "
                          }, False, Hue.Black)
        buffer.WriteTextCentered(22, "SPACE to start", True, Hue.Orange)
    End Sub

    Public Sub Initialize() Implements IProcessor.Initialize
    End Sub

    Public Function ProcessCommand(command As Command) As UIState Implements IProcessor.ProcessCommand
        If command = Command.Green OrElse command = Command.Blue Then
            Return UIState.InPlay
        End If
        Return UIState.Prolog
    End Function
End Class
