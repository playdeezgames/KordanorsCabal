Module RoutePopulator
    Sub PopulateRoutesRecord(connection As SqliteConnection, RouteId As Long, LocationId As Long, DirectionId As Long, RouteTypeId As Long, ToLocationId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Routes}]([{Columns.RouteIdColumn}], [{Columns.LocationIdColumn}], [{Columns.DirectionIdColumn}], [{Columns.RouteTypeIdColumn}], [{Columns.ToLocationIdColumn}]) VALUES (@{Columns.RouteIdColumn}, @{Columns.LocationIdColumn}, @{Columns.DirectionIdColumn}, @{Columns.RouteTypeIdColumn}, @{Columns.ToLocationIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.RouteIdColumn}", RouteId)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.DirectionIdColumn}", DirectionId)
            command.Parameters.AddWithValue($"@{Columns.RouteTypeIdColumn}", RouteTypeId)
            command.Parameters.AddWithValue($"@{Columns.ToLocationIdColumn}", ToLocationId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateRoutes(connection As SqliteConnection)
    End Sub
End Module
