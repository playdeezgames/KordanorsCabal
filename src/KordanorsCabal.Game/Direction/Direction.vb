Imports System.Runtime.CompilerServices

Public Enum Direction
    None
    North
    East
    South
    West
    Up
    Down
    Inward
    Outward
End Enum
Module DirectionExtensions
    <Extension>
    Function Opposite(direction As Direction) As Direction
        Return DirectionDescriptors(direction).Opposite
    End Function
End Module
