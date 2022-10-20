Module DungeonLevelPopulator
    Private Sub PopulateDungeonLevelsRecord(connection As SqliteConnection, DungeonLevelId As Long, DungeonLevelName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.DungeonLevels}]([{Columns.DungeonLevelIdColumn}], [{Columns.DungeonLevelNameColumn}]) VALUES (@{Columns.DungeonLevelIdColumn}, @{Columns.DungeonLevelNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.DungeonLevelIdColumn}", DungeonLevelId)
            command.Parameters.AddWithValue($"@{Columns.DungeonLevelNameColumn}", DungeonLevelName)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Const DungeonLevel1 = 1L
    Friend Const DungeonLevel2 = 2L
    Friend Const DungeonLevel3 = 3L
    Friend Const DungeonLevel4 = 4L
    Friend Const DungeonLevel5 = 5L
    Friend Const DungeonLevel6 = 6L
    Friend Sub PopulateDungeonLevels(connection As SqliteConnection)
        PopulateDungeonLevelsRecord(connection, DungeonLevel1, "Level I")
        PopulateDungeonLevelsRecord(connection, DungeonLevel2, "Level II")
        PopulateDungeonLevelsRecord(connection, DungeonLevel3, "Level III")
        PopulateDungeonLevelsRecord(connection, DungeonLevel4, "Level IV")
        PopulateDungeonLevelsRecord(connection, DungeonLevel5, "Level V")
        PopulateDungeonLevelsRecord(connection, DungeonLevel6, "The Moon")
    End Sub

End Module
