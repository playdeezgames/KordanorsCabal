﻿Module ItemTypeStatisticTypePopulator
    Private Sub PopulateItemTypeStatisticTypesRecord(connection As SqliteConnection, ItemTypeStatisticTypeId As Long, ItemTypeStatisticTypeName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemTypeStatisticTypes}]([{Columns.ItemTypeStatisticTypeIdColumn}], [{Columns.ItemTypeStatisticTypeNameColumn}]) VALUES (@{Columns.ItemTypeStatisticTypeIdColumn}, @{Columns.ItemTypeStatisticTypeNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeStatisticTypeIdColumn}", ItemTypeStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeStatisticTypeNameColumn}", ItemTypeStatisticTypeName)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Const ItemTypeStatisticType1 = 1L
    Friend Const ItemTypeStatisticType2 = 2L
    Friend Const ItemTypeStatisticType3 = 3L
    Friend Const ItemTypeStatisticType4 = 4L
    Friend Const ItemTypeStatisticType5 = 5L
    Friend Const ItemTypeStatisticType6 = 6L
    Friend Const ItemTypeStatisticType7 = 7L
    Friend Const ItemTypeStatisticType8 = 8L
    Friend Sub PopulateItemTypeStatisticTypes(connection As SqliteConnection)
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType1, "Encumbrance")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType2, "AttackDice")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType3, "MaximumDamage")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType4, "DefendDice")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType5, "MaximumDurability")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType6, "Offer")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType7, "Price")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType8, "RepairPrice")
    End Sub

End Module