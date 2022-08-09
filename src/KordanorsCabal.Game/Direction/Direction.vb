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
    Public Function ToDescriptor(direction As Direction) As DirectionDescriptor
        Return DirectionDescriptors(direction)
    End Function
End Module
