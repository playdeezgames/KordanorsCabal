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
Public Module DirectionExtensions
    <Extension>
    Friend Function Opposite(direction As Direction) As Direction
        Return DirectionDescriptors(direction).Opposite
    End Function
    <Extension>
    Public Function Name(direction As Direction) As String
        Return DirectionDescriptors(direction).Name
    End Function
    <Extension>
    Public Function IsCardinal(direction As Direction) As Boolean
        Return DirectionDescriptors(direction).IsCardinal
    End Function
End Module
