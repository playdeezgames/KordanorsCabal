Public Class Maze(Of TDirection)
    ReadOnly Property Columns As Long
    ReadOnly Property Rows As Long
    Private _cells As New List(Of MazeCell(Of TDirection))
    Sub New(columns As Long, rows As Long, directions As Dictionary(Of TDirection, MazeDirection(Of TDirection)))
        Me.Columns = columns
        Me.Rows = rows
        While _cells.Count < columns * rows
            _cells.Add(New MazeCell(Of TDirection))
        End While
        For column = 0 To columns - 1
            For row = 0 To rows - 1
                Dim cell = GetCell(column, row)
                For Each direction In directions
                    If Not cell.HasNeighbor(direction.Key) Then
                        Dim nextColumn = column + direction.Value.DeltaX
                        Dim nextRow = row + direction.Value.DeltaY
                        Dim nextCell = GetCell(nextColumn, nextRow)
                        If nextCell IsNot Nothing Then
                            cell.SetNeighbor(direction.Key, nextCell)
                            nextCell.SetNeighbor(direction.Value.Opposite, cell)
                            Dim door = New MazeDoor
                            cell.SetDoor(direction.Key, door)
                            nextCell.SetDoor(direction.Value.Opposite, door)
                        End If
                    End If
                Next
            Next
        Next
    End Sub
    Private Sub Reset()
        For Each cell In _cells
            cell.Reset()
        Next
    End Sub
    Sub Generate()
        Reset()
        Dim cell = GetCell(RNG.FromRange(0, CInt(Columns) - 1), RNG.FromRange(0, CInt(Rows) - 1))
        Dim inside As New List(Of MazeCell(Of TDirection)) From {cell}
        Dim frontier = cell.Neighbors
        While frontier.Any
            cell = RNG.FromList(frontier)
            Dim direction = RNG.FromList(cell.Directions.Where(Function(x) inside.Contains(cell.GetNeighbor(x))).ToList)
            cell.GetDoor(direction).Open = True
            inside.Add(cell)
            frontier.Remove(cell)
            For Each neighbor In cell.Neighbors.Where(Function(x) Not inside.Contains(x) AndAlso Not frontier.Contains(x))
                frontier.Add(neighbor)
            Next
        End While
    End Sub
    Function GetCell(column As Long, row As Long) As MazeCell(Of TDirection)
        If column >= 0 AndAlso row >= 0 AndAlso column < Columns AndAlso row < Rows Then
            Return _cells(CInt(column + row * Columns))
        End If
        Return Nothing
    End Function
End Class
