Module RouteTypePopulator
    Sub PopulateRouteTypesRecord(connection As SqliteConnection, RouteTypeId As Long, Abbreviation As String, IsSingleUse As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.RouteTypes}]([{Columns.RouteTypeIdColumn}], [{Columns.AbbreviationColumn}], [{Columns.IsSingleUseColumn}]) VALUES (@{Columns.RouteTypeIdColumn}, @{Columns.AbbreviationColumn}, @{Columns.IsSingleUseColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.RouteTypeIdColumn}", RouteTypeId)
            command.Parameters.AddWithValue($"@{Columns.AbbreviationColumn}", Abbreviation)
            command.Parameters.AddWithValue($"@{Columns.IsSingleUseColumn}", IsSingleUse)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Public Const RouteType1 = 1L
    Public Const RouteType2 = 2L
    Public Const RouteType3 = 3L
    Public Const RouteType4 = 4L
    Public Const RouteType5 = 5L
    Public Const RouteType6 = 6L
    Public Const RouteType7 = 7L
    Public Const RouteType8 = 8L
    Public Const RouteType9 = 9L
    Public Const RouteType10 = 10L
    Public Const RouteType11 = 11L
    Sub PopulateRouteTypes(connection As SqliteConnection)
        PopulateRouteTypesRecord(connection, RouteType1, "  ", 0)
        PopulateRouteTypesRecord(connection, RouteType2, "  ", 0)
        PopulateRouteTypesRecord(connection, RouteType3, "  ", 0)
        PopulateRouteTypesRecord(connection, RouteType4, "FE", 0)
        PopulateRouteTypesRecord(connection, RouteType5, "CU", 0)
        PopulateRouteTypesRecord(connection, RouteType6, "AG", 0)
        PopulateRouteTypesRecord(connection, RouteType7, "AU", 0)
        PopulateRouteTypesRecord(connection, RouteType8, "PT", 0)
        PopulateRouteTypesRecord(connection, RouteType9, "EO", 0)
        PopulateRouteTypesRecord(connection, RouteType10, "  ", 1)
        PopulateRouteTypesRecord(connection, RouteType11, "  ", 0)
    End Sub
    Sub PopulateRouteTypeLocksRecord(connection As SqliteConnection, RouteTypeId As Long, UnlockedRouteTypeId As Long, UnlockItemTypeId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.RouteTypeLocks}]([{Columns.RouteTypeIdColumn}], [{Columns.UnlockedRouteTypeIdColumn}], [{Columns.UnlockItemTypeIdColumn}]) VALUES (@{Columns.RouteTypeIdColumn}, @{Columns.UnlockedRouteTypeIdColumn}, @{Columns.UnlockItemTypeIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.RouteTypeIdColumn}", RouteTypeId)
            command.Parameters.AddWithValue($"@{Columns.UnlockedRouteTypeIdColumn}", UnlockedRouteTypeId)
            command.Parameters.AddWithValue($"@{Columns.UnlockItemTypeIdColumn}", UnlockItemTypeId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateRouteTypeLocks(connection As SqliteConnection)
        PopulateRouteTypeLocksRecord(connection, RouteType4, RouteType2, ItemType1)
        PopulateRouteTypeLocksRecord(connection, RouteType5, RouteType2, ItemType2)
        PopulateRouteTypeLocksRecord(connection, RouteType6, RouteType2, ItemType3)
        PopulateRouteTypeLocksRecord(connection, RouteType7, RouteType2, ItemType4)
        PopulateRouteTypeLocksRecord(connection, RouteType8, RouteType2, ItemType5)
        PopulateRouteTypeLocksRecord(connection, RouteType9, RouteType2, ItemType6)
    End Sub
End Module
