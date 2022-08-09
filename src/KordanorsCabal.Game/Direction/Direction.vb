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
    Public Function Opposite(direction As Direction) As Direction
        Return DirectionDescriptors(direction).Opposite
    End Function
    <Extension>
    Public Function NextDirection(direction As Direction) As Direction?
        Return DirectionDescriptors(direction).NextDirection
    End Function
    <Extension>
    Public Function PreviousDirection(direction As Direction) As Direction?
        Return DirectionDescriptors(direction).PreviousDirection
    End Function
    <Extension>
    Public Function Abbreviation(direction As Direction) As String
        Return DirectionDescriptors(direction).Abbreviation
    End Function
    <Extension>
    Public Function ToDescriptor(direction As Direction) As DirectionDescriptor
        Return DirectionDescriptors(direction)
    End Function
    <Extension>
    Public Function ToDirection(directionId As Long) As Direction
        Return CType(directionId, Direction)
    End Function
End Module
