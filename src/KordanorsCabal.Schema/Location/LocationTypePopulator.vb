Public Module LocationTypePopulator
    Private Sub PopulateLocationTypesRecord(connection As SqliteConnection, LocationTypeId As Long, LocationTypeName As String, IsDungeon As Long, CanMap As Long, RequiresMP As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.LocationTypes}]([{Columns.LocationTypeIdColumn}], [{Columns.LocationTypeNameColumn}], [{Columns.IsDungeonColumn}], [{Columns.CanMapColumn}], [{Columns.RequiresMPColumn}]) VALUES (@{Columns.LocationTypeIdColumn}, @{Columns.LocationTypeNameColumn}, @{Columns.IsDungeonColumn}, @{Columns.CanMapColumn}, @{Columns.RequiresMPColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.LocationTypeIdColumn}", LocationTypeId)
            command.Parameters.AddWithValue($"@{Columns.LocationTypeNameColumn}", LocationTypeName)
            command.Parameters.AddWithValue($"@{Columns.IsDungeonColumn}", IsDungeon)
            command.Parameters.AddWithValue($"@{Columns.CanMapColumn}", CanMap)
            command.Parameters.AddWithValue($"@{Columns.RequiresMPColumn}", RequiresMP)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Public Const LocationType1 = 1L
    Public Const LocationType2 = 2L
    Public Const LocationType3 = 3L
    Public Const LocationType4 = 4L
    Public Const LocationType5 = 5L
    Public Const LocationType6 = 6L
    Public Const LocationType7 = 7L
    Public Const LocationType8 = 8L
    Friend Sub PopulateLocationTypes(connection As SqliteConnection)
        PopulateLocationTypesRecord(connection, LocationType1, "Town Square", 0, 0, 0)
        PopulateLocationTypesRecord(connection, LocationType2, "Town", 0, 0, 0)
        PopulateLocationTypesRecord(connection, LocationType3, "Church Entrance", 0, 0, 0)
        PopulateLocationTypesRecord(connection, LocationType4, "Dungeon", 1, 1, 1)
        PopulateLocationTypesRecord(connection, LocationType5, "Dungeon Dead End", 1, 1, 1)
        PopulateLocationTypesRecord(connection, LocationType6, "Dungeon Boss", 1, 1, 1)
        PopulateLocationTypesRecord(connection, LocationType7, "Cellar", 1, 0, 1)
        PopulateLocationTypesRecord(connection, LocationType8, "Moon", 1, 1, 1)
    End Sub
End Module
