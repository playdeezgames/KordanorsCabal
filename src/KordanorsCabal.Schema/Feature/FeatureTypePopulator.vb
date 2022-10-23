Public Module FeatureTypePopulator
    Private Sub PopulateFeatureTypesRecord(connection As SqliteConnection, FeatureTypeId As Long, FeatureTypeName As String, LocationTypeId As Long, InteractionMode As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.FeatureTypes}]([{Columns.FeatureTypeIdColumn}], [{Columns.FeatureTypeNameColumn}], [{Columns.LocationTypeIdColumn}], [{Columns.InteractionModeColumn}]) VALUES (@{Columns.FeatureTypeIdColumn}, @{Columns.FeatureTypeNameColumn}, @{Columns.LocationTypeIdColumn}, @{Columns.InteractionModeColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.FeatureTypeIdColumn}", FeatureTypeId)
            command.Parameters.AddWithValue($"@{Columns.FeatureTypeNameColumn}", FeatureTypeName)
            command.Parameters.AddWithValue($"@{Columns.LocationTypeIdColumn}", LocationTypeId)
            command.Parameters.AddWithValue($"@{Columns.InteractionModeColumn}", InteractionMode)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Public Const FeatureType1 = 1L
    Public Const FeatureType2 = 2L
    Public Const FeatureType3 = 3L
    Public Const FeatureType4 = 4L
    Public Const FeatureType5 = 5L
    Public Const FeatureType6 = 6L
    Public Const FeatureType7 = 7L
    Public Const FeatureType8 = 8L
    Public Const FeatureType9 = 9L
    Public Const InteractionMode4 = 4
    Public Const InteractionMode5 = 5
    Public Const InteractionMode6 = 6
    Public Const InteractionMode7 = 7
    Public Const InteractionMode8 = 8
    Public Const InteractionMode9 = 9
    Public Const InteractionMode10 = 10
    Public Const InteractionMode11 = 11
    Public Const InteractionMode12 = 12
    Friend Sub PopulateFeatureTypes(connection As SqliteConnection)
        PopulateFeatureTypesRecord(connection, FeatureType1, "Zooperdan the Elder", LocationType1, InteractionMode4)
        PopulateFeatureTypesRecord(connection, FeatureType2, "Graham the Innkeeper", LocationType2, InteractionMode5)
        PopulateFeatureTypesRecord(connection, FeatureType3, "Yermom the Drunk", LocationType2, InteractionMode6)
        PopulateFeatureTypesRecord(connection, FeatureType4, "Sander the Chicken", LocationType2, InteractionMode7)
        PopulateFeatureTypesRecord(connection, FeatureType5, """Honest"" Dan", LocationType2, InteractionMode8)
        PopulateFeatureTypesRecord(connection, FeatureType6, "Marcus the Black Mage", LocationType2, InteractionMode9)
        PopulateFeatureTypesRecord(connection, FeatureType7, "Samuli the Blacksmith", LocationType2, InteractionMode10)
        PopulateFeatureTypesRecord(connection, FeatureType8, "Nihilist Healer Marten", LocationType2, InteractionMode12)
        PopulateFeatureTypesRecord(connection, FeatureType9, "David the Constable", LocationType2, InteractionMode11)
    End Sub
End Module
