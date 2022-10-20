Module ShoppeTypePopulator
    Private Sub PopulateShoppeTypesRecord(connection As SqliteConnection, ShoppeTypeId As Long, ShoppeTypeName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ShoppeTypes}]([{Columns.ShoppeTypeIdColumn}], [{Columns.ShoppeTypeNameColumn}]) VALUES (@{Columns.ShoppeTypeIdColumn}, @{Columns.ShoppeTypeNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ShoppeTypeIdColumn}", ShoppeTypeId)
            command.Parameters.AddWithValue($"@{Columns.ShoppeTypeNameColumn}", ShoppeTypeName)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Const ShoppeType1 = 1L
    Friend Const ShoppeType2 = 2L
    Friend Const ShoppeType3 = 3L
    Friend Const ShoppeType4 = 4L
    Friend Const ShoppeType5 = 5L
    Friend Sub PopulateShoppeTypes(connection As SqliteConnection)
        PopulateShoppeTypesRecord(connection, ShoppeType1, "Magic")
        PopulateShoppeTypesRecord(connection, ShoppeType2, "Blacksmith")
        PopulateShoppeTypesRecord(connection, ShoppeType3, "Innkeeper")
        PopulateShoppeTypesRecord(connection, ShoppeType4, "Healer")
        PopulateShoppeTypesRecord(connection, ShoppeType5, "Black Market")
    End Sub

End Module
