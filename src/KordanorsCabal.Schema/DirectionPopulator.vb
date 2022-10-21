Module DirectionPopulator
    Private Sub PopulateDirectionsRecord(connection As SqliteConnection, DirectionId As Long, DirectionName As String, Abbreviation As String, IsCardinal As Long, PreviousDirectionId As Long?, OppositeDirectionId As Long, NextDirectionId As Long?)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Directions}]([{Columns.DirectionIdColumn}], [{Columns.DirectionNameColumn}], [{Columns.AbbreviationColumn}], [{Columns.IsCardinalColumn}], [{Columns.PreviousDirectionIdColumn}], [{Columns.OppositeDirectionIdColumn}], [{Columns.NextDirectionIdColumn}]) VALUES (@{Columns.DirectionIdColumn}, @{Columns.DirectionNameColumn}, @{Columns.AbbreviationColumn}, @{Columns.IsCardinalColumn}, @{Columns.PreviousDirectionIdColumn}, @{Columns.OppositeDirectionIdColumn}, @{Columns.NextDirectionIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.DirectionIdColumn}", DirectionId)
            command.Parameters.AddWithValue($"@{Columns.DirectionNameColumn}", DirectionName)
            command.Parameters.AddWithValue($"@{Columns.AbbreviationColumn}", Abbreviation)
            command.Parameters.AddWithValue($"@{Columns.IsCardinalColumn}", IsCardinal)
            command.Parameters.AddWithValue($"@{Columns.PreviousDirectionIdColumn}", If(PreviousDirectionId Is Nothing, CObj(DBNull.Value), PreviousDirectionId))
            command.Parameters.AddWithValue($"@{Columns.OppositeDirectionIdColumn}", OppositeDirectionId)
            command.Parameters.AddWithValue($"@{Columns.NextDirectionIdColumn}", If(NextDirectionId Is Nothing, CObj(DBNull.Value), NextDirectionId))
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Const Direction1 = 1L
    Friend Const Direction2 = 2L
    Friend Const Direction3 = 3L
    Friend Const Direction4 = 4L
    Friend Const Direction5 = 5L
    Friend Const Direction6 = 6L
    Friend Const Direction7 = 7L
    Friend Const Direction8 = 8L
    Friend Sub PopulateDirections(connection As SqliteConnection)
        PopulateDirectionsRecord(connection, Direction1, "North", "N", 1, Direction4, Direction3, Direction2)
        PopulateDirectionsRecord(connection, Direction2, "East", "E", 1, Direction1, Direction4, Direction3)
        PopulateDirectionsRecord(connection, Direction3, "South", "S", 1, Direction2, Direction1, Direction4)
        PopulateDirectionsRecord(connection, Direction4, "West", "W", 1, Direction3, Direction2, Direction1)
        PopulateDirectionsRecord(connection, Direction5, "Up", "U", 0, Nothing, Direction6, Nothing)
        PopulateDirectionsRecord(connection, Direction6, "Down", "D", 0, Nothing, Direction5, Nothing)
        PopulateDirectionsRecord(connection, Direction7, "In", "In", 0, Nothing, Direction8, Nothing)
        PopulateDirectionsRecord(connection, Direction8, "Out", "Out", 0, Nothing, Direction7, Nothing)
    End Sub
End Module
