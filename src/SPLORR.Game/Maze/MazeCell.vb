Public Class MazeCell(Of TDirection)
    Private ReadOnly _neighbors As New Dictionary(Of TDirection, MazeCell(Of TDirection))
    Private ReadOnly _doors As New Dictionary(Of TDirection, MazeDoor)
    Sub New()

    End Sub
    Friend Function HasNeighbor(direction As TDirection) As Boolean
        Return _neighbors.ContainsKey(direction)
    End Function

    Friend Sub SetNeighbor(direction As TDirection, nextCell As MazeCell(Of TDirection))
        _neighbors.Remove(direction)
        _neighbors.Add(direction, nextCell)
    End Sub
    Friend Function GetNeighbor(direction As TDirection) As MazeCell(Of TDirection)
        Dim cell As MazeCell(Of TDirection) = Nothing
        _neighbors.TryGetValue(direction, cell)
        Return cell
    End Function
    Function GetDoor(direction As TDirection) As MazeDoor
        Dim door As MazeDoor = Nothing
        _doors.TryGetValue(direction, door)
        Return door
    End Function
    ReadOnly Property Neighbors As List(Of MazeCell(Of TDirection))
        Get
            Return _neighbors.Select(Function(x) x.Value).ToList
        End Get
    End Property
    ReadOnly Property Directions As List(Of TDirection)
        Get
            Return _neighbors.Select(Function(x) x.Key).ToList
        End Get
    End Property

    Friend Sub SetDoor(direction As TDirection, door As MazeDoor)
        _doors.Remove(direction)
        _doors.Add(direction, door)
    End Sub

    Friend Sub Reset()
        For Each door In _doors.Where(Function(x) x.Value.Open)
            door.Value.Open = False
        Next
    End Sub

    Public Function OpenDoorCount() As Integer
        Return _doors.Where(Function(x) x.Value.Open).Count
    End Function
End Class
