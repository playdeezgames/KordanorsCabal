Module ItemStatisticTypePopulator
    Private Sub PopulateItemStatisticTypesRecord(connection As SqliteConnection, ItemStatisticTypeId As Long, DefaultValue As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemStatisticTypes}]([{Columns.ItemStatisticTypeIdColumn}], [{Columns.DefaultValueColumn}]) VALUES (@{Columns.ItemStatisticTypeIdColumn}, @{Columns.DefaultValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemStatisticTypeIdColumn}", ItemStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.DefaultValueColumn}", DefaultValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Const ItemStatisticType1 = 1L
    Friend Sub PopulateItemStatisticTypes(connection As SqliteConnection)
        PopulateItemStatisticTypesRecord(connection, ItemStatisticType1, 0)
    End Sub

End Module
