﻿Module ItemKindPopulator
    Public Const ItemKind1 = 1L
    Friend Sub PopulateItemKinds(connection As SqliteConnection)
        PopulateItemKindsRecord(connection, ItemKind1, "Trophy")
    End Sub
    Private Sub PopulateItemKindsRecord(connection As SqliteConnection, itemKindId As Long, itemKindName As String)
        Using command = New SqliteCommand(
                $"INSERT INTO [{Tables.ItemKinds}]
                (
                    [{Columns.ItemKindIdColumn}], 
                    [{Columns.ItemKindNameColumn}]
                ) 
                VALUES 
                (
                    @{Columns.ItemKindIdColumn}, 
                    @{Columns.ItemKindNameColumn}
                );", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemKindIdColumn}", itemKindId)
            command.Parameters.AddWithValue($"@{Columns.ItemKindNameColumn}", itemKindName)
            command.ExecuteNonQuery()
        End Using
    End Sub
End Module
