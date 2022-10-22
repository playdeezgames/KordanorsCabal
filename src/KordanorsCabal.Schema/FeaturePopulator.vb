Module FeaturePopulator
    Sub PopulateFeaturesRecord(connection As SqliteConnection, FeatureId As Long, FeatureTypeId As Long, LocationId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Features}]([{Columns.FeatureIdColumn}], [{Columns.FeatureTypeIdColumn}], [{Columns.LocationIdColumn}]) VALUES (@{Columns.FeatureIdColumn}, @{Columns.FeatureTypeIdColumn}, @{Columns.LocationIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.FeatureIdColumn}", FeatureId)
            command.Parameters.AddWithValue($"@{Columns.FeatureTypeIdColumn}", FeatureTypeId)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateFeatures(connection As SqliteConnection)
    End Sub

End Module
