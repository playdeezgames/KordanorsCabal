Module LocationPopulator

    Sub PopulateLocationsRecord(connection As SqliteConnection, LocationId As Long, LocationTypeId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Locations}]([{Columns.LocationIdColumn}], [{Columns.LocationTypeIdColumn}]) VALUES (@{Columns.LocationIdColumn}, @{Columns.LocationTypeIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.LocationTypeIdColumn}", LocationTypeId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateLocations(connection As SqliteConnection)
    End Sub
    Sub PopulateLocationDungeonLevelsRecord(connection As SqliteConnection, LocationId As Long, DungeonLevelId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.LocationDungeonLevels}]([{Columns.LocationIdColumn}], [{Columns.DungeonLevelIdColumn}]) VALUES (@{Columns.LocationIdColumn}, @{Columns.DungeonLevelIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.DungeonLevelIdColumn}", DungeonLevelId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateLocationDungeonLevels(connection As SqliteConnection)
    End Sub
    Sub PopulateLocationStatisticsRecord(connection As SqliteConnection, LocationId As Long, LocationStatisticTypeId As Long, StatisticValue As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.LocationStatistics}]([{Columns.LocationIdColumn}], [{Columns.LocationStatisticTypeIdColumn}], [{Columns.StatisticValueColumn}]) VALUES (@{Columns.LocationIdColumn}, @{Columns.LocationStatisticTypeIdColumn}, @{Columns.StatisticValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.LocationStatisticTypeIdColumn}", LocationStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.StatisticValueColumn}", StatisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateLocationStatistics(connection As SqliteConnection)
    End Sub
End Module
