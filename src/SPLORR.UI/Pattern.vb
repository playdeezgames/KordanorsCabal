Imports System.Runtime.CompilerServices

Public Enum Pattern
    At
    A
    B
    C
    D
    E
    F
    G
    H
    I
    J
    K
    L
    M
    N
    O
    P
    Q
    R
    S
    T
    U
    V
    W
    X
    Y
    Z
    OpenBracket
    Pound
    CloseBracket
    UpArrow
    LeftArrow
    Space
    Bang
    Quote
    Octothorpe
    Dollar
    Percent
    Ampersand
    Apostrophe
    OpenParenthesis
    CloseParenthesis
    Asterisk
    Plus
    Comma
    Minus
    Dot
    Slash
    Zero
    One
    Two
    Three
    Four
    Five
    Six
    Seven
    Eight
    Nine
    Colon
    Semicolon
    LessThan
    Equal
    GreaterThan
    Question
    Horizontal5
    Spade
    Vertical4
    Horizontal4
    Horizontal3
    Horizontal2
    Horizontal6
    Vertical3
    Vertical6
    TopRightCurve
    BottomLeftCurve
    BottomRightCurve
    BottomLeftCorner
    DownwardDiagonal
    UpwardDiagonal
    TopLeftCorner
    TopRightCorner
    FilledCircle
    Horizontal7
    Heart
    Vertical2
    TopLeftCurve
    CrossDiagonals
    EmptyCircle
    Club
    Vertical7
    Diamond
    Cross
    LeftDither
    Vertical5
    Pi
    TopRightSolid
    Blank
    Vertical1234
    Horizontal5678
    Horizontal1
    Horizontal8
    Vertical1
    Dither
    Vertical8
    BottomDither
    UpperLeftSolid
    Vertical78
    TUpRightDown
    BottomRightBlock
    ElbowUpRight
    ElbowDownLeft
    Horizontal78
    ElbowDownRight
    TUpRightLeft
    TRightDownLeft
    TUpDownLeft
    Vertical12
    Vertical123
    Vertical678
    Horizontal12
    Horizontal123
    Horizontal678
    BottomRightCorner
    BottomLeftBlock
    TopRightBlock
    ElbowUpLeft
    TopLeftBlock
    TopLeftBottomRightBlocks
End Enum
Public Module PatternUtility
    Public ReadOnly CharacterPattern As IReadOnlyDictionary(Of Char, Pattern) =
        New Dictionary(Of Char, Pattern) From
        {
            {"@"c, Pattern.At},
            {"A"c, Pattern.A},
            {"B"c, Pattern.B},
            {"C"c, Pattern.C},
            {"D"c, Pattern.D},
            {"E"c, Pattern.E},
            {"F"c, Pattern.F},
            {"G"c, Pattern.G},
            {"H"c, Pattern.H},
            {"I"c, Pattern.I},
            {"J"c, Pattern.J},
            {"K"c, Pattern.K},
            {"L"c, Pattern.L},
            {"M"c, Pattern.M},
            {"N"c, Pattern.N},
            {"O"c, Pattern.O},
            {"P"c, Pattern.P},
            {"Q"c, Pattern.Q},
            {"R"c, Pattern.R},
            {"S"c, Pattern.S},
            {"T"c, Pattern.T},
            {"U"c, Pattern.U},
            {"V"c, Pattern.V},
            {"W"c, Pattern.W},
            {"X"c, Pattern.X},
            {"Y"c, Pattern.Y},
            {"Z"c, Pattern.Z},
            {"a"c, Pattern.A},
            {"b"c, Pattern.B},
            {"c"c, Pattern.C},
            {"d"c, Pattern.D},
            {"e"c, Pattern.E},
            {"f"c, Pattern.F},
            {"g"c, Pattern.G},
            {"h"c, Pattern.H},
            {"i"c, Pattern.I},
            {"j"c, Pattern.J},
            {"k"c, Pattern.K},
            {"l"c, Pattern.L},
            {"m"c, Pattern.M},
            {"n"c, Pattern.N},
            {"o"c, Pattern.O},
            {"p"c, Pattern.P},
            {"q"c, Pattern.Q},
            {"r"c, Pattern.R},
            {"s"c, Pattern.S},
            {"t"c, Pattern.T},
            {"u"c, Pattern.U},
            {"v"c, Pattern.V},
            {"w"c, Pattern.W},
            {"x"c, Pattern.X},
            {"y"c, Pattern.Y},
            {"z"c, Pattern.Z},
            {"["c, Pattern.OpenBracket},
            {"£"c, Pattern.Pound},
            {"]"c, Pattern.CloseBracket},
            {"↑"c, Pattern.UpArrow},
            {"←"c, Pattern.LeftArrow},
            {" "c, Pattern.Space},
            {"!"c, Pattern.Bang},
            {""""c, Pattern.Quote},
            {"#"c, Pattern.Octothorpe},
            {"$"c, Pattern.Dollar},
            {"%"c, Pattern.Percent},
            {"&"c, Pattern.Ampersand},
            {"'"c, Pattern.Apostrophe},
            {"("c, Pattern.OpenParenthesis},
            {")"c, Pattern.CloseParenthesis},
            {"*"c, Pattern.Asterisk},
            {"+"c, Pattern.Plus},
            {","c, Pattern.Comma},
            {"-"c, Pattern.Minus},
            {"."c, Pattern.Dot},
            {"/"c, Pattern.Slash},
            {"0"c, Pattern.Zero},
            {"1"c, Pattern.One},
            {"2"c, Pattern.Two},
            {"3"c, Pattern.Three},
            {"4"c, Pattern.Four},
            {"5"c, Pattern.Five},
            {"6"c, Pattern.Six},
            {"7"c, Pattern.Seven},
            {"8"c, Pattern.Eight},
            {"9"c, Pattern.Nine},
            {":"c, Pattern.Colon},
            {";"c, Pattern.Semicolon},
            {"<"c, Pattern.LessThan},
            {"="c, Pattern.Equal},
            {">"c, Pattern.GreaterThan},
            {"?"c, Pattern.Question},
            {"│"c, Pattern.Horizontal5},
            {"^"c, Pattern.UpArrow},
            {"♠"c, Pattern.Spade},'{" "c, Pattern.Vertical4},{" "c, Pattern.Horizontal4},{" "c, Pattern.Horizontal3},{" "c, Pattern.Horizontal2},{" "c, Pattern.Horizontal6},{" "c, Pattern.Vertical3},{" "c, Pattern.Vertical6},
            {"╮"c, Pattern.TopRightCurve},
            {"╰"c, Pattern.BottomLeftCurve},
            {"╯"c, Pattern.BottomRightCurve},
            {"╚"c, Pattern.BottomLeftCorner},
            {"╲"c, Pattern.DownwardDiagonal},
            {"╱"c, Pattern.UpwardDiagonal},
            {"╔"c, Pattern.TopLeftCorner},
            {"╗"c, Pattern.TopRightCorner},
            {"●"c, Pattern.FilledCircle},'{" "c, Pattern.Horizontal7},
            {"♥"c, Pattern.Heart},'{" "c, Pattern.Vertical2},
            {"╭"c, Pattern.TopLeftCurve},
            {"╳"c, Pattern.CrossDiagonals},
            {"○"c, Pattern.EmptyCircle},
            {"♣"c, Pattern.Club},'{" "c, Pattern.Vertical7},
            {"♦"c, Pattern.Diamond},
            {"┼"c, Pattern.Cross},'{" "c, Pattern.LeftDither},{"─"c, Pattern.Vertical5},{" "c, Pattern.Pi},{" "c, Pattern.UpperRightSolid},{" "c, Pattern.Blank},{" "c, Pattern.Vertical1234},{" "c, Pattern.Horizontal5678},{" "c, Pattern.Horizontal1},{" "c, Pattern.Horizontal8},{" "c, Pattern.Vertical1},{" "c, Pattern.Dither},{" "c, Pattern.Vertical8},{" "c, Pattern.BottomDither},{" "c, Pattern.UpperLeftSolid},{" "c, Pattern.Vertical78},
            {"├"c, Pattern.TUpRightDown},'{" "c, Pattern.LowerRightBlock},
            {"└"c, Pattern.ElbowUpRight},
            {"┐"c, Pattern.ElbowDownLeft},'{" "c, Pattern.Horizontal78},
            {"┌"c, Pattern.ElbowDownRight},
            {"┴"c, Pattern.TUpRightLeft},
            {"┬"c, Pattern.TRightDownLeft},
            {"┤"c, Pattern.TUpDownLeft},'{" "c, Pattern.Vertical12},{" "c, Pattern.Vertical123},{" "c, Pattern.Vertical678},{" "c, Pattern.Horizontal12},{" "c, Pattern.Horizontal123},{" "c, Pattern.Horizontal678},
            {"╝"c, Pattern.BottomRightCorner},'{" "c, Pattern.BottomLeftBlock},{" "c, Pattern.TopRightBlock},
            {"┘"c, Pattern.ElbowUpLeft}',{" "c, Pattern.TopLeftBlock},{" "c, Pattern.TopLeftBottomRightBlocks}
        }
End Module
