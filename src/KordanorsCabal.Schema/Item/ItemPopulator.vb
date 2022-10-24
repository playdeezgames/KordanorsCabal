Module ItemPopulator
    Sub PopulateItemsRecord(connection As SqliteConnection, ItemId As Long, ItemTypeId As Long, ItemName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Items}]([{Columns.ItemIdColumn}], [{Columns.ItemTypeIdColumn}], [{Columns.ItemNameColumn}]) VALUES (@{Columns.ItemIdColumn}, @{Columns.ItemTypeIdColumn}, @{Columns.ItemNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemIdColumn}", ItemId)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeIdColumn}", ItemTypeId)
            command.Parameters.AddWithValue($"@{Columns.ItemNameColumn}", If(ItemName Is Nothing, CObj(DBNull.Value), CObj(ItemName)))
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateItems(connection As SqliteConnection)
    End Sub
    Sub PopulateInventoriesRecord(connection As SqliteConnection, InventoryId As Long, CharacterId As Long?, LocationId As Long?)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Inventories}]([{Columns.InventoryIdColumn}], [{Columns.CharacterIdColumn}], [{Columns.LocationIdColumn}]) VALUES (@{Columns.InventoryIdColumn}, @{Columns.CharacterIdColumn}, @{Columns.LocationIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.InventoryIdColumn}", InventoryId)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", If(CharacterId Is Nothing, CObj(DBNull.Value), CharacterId))
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", If(LocationId Is Nothing, CObj(DBNull.Value), LocationId))
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateInventories(connection As SqliteConnection)
    End Sub
    Sub PopulateInventoryItemsRecord(connection As SqliteConnection, InventoryId As Long, ItemId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.InventoryItems}]([{Columns.InventoryIdColumn}], [{Columns.ItemIdColumn}]) VALUES (@{Columns.InventoryIdColumn}, @{Columns.ItemIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.InventoryIdColumn}", InventoryId)
            command.Parameters.AddWithValue($"@{Columns.ItemIdColumn}", ItemId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateInventoryItems(connection As SqliteConnection)
    End Sub
    Sub PopulateItemStatisticsRecord(connection As SqliteConnection, ItemId As Long, ItemStatisticTypeId As Long, StatisticValue As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemStatistics}]([{Columns.ItemIdColumn}], [{Columns.CharacterStatisticTypeIdColumn}], [{Columns.StatisticValueColumn}]) VALUES (@{Columns.ItemIdColumn}, @{Columns.CharacterStatisticTypeIdColumn}, @{Columns.StatisticValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemIdColumn}", ItemId)
            command.Parameters.AddWithValue($"@{Columns.CharacterStatisticTypeIdColumn}", ItemStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.StatisticValueColumn}", StatisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateItemStatistics(connection As SqliteConnection)
    End Sub

End Module
