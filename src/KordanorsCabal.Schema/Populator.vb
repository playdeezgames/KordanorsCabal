Public Module Populator
    Sub Populate(connection As SqliteConnection)
        PopulateCharacterTypes(connection)
        PopulateCharacterStatisticTypes(connection)
        PopulateCharacterTypeInitialStatistics(connection)
        PopulateCharacterTypeAttackTypes(connection)
        PopulateItemKinds(connection)
        PopulateItemTypes(connection)
        PopulateCharacterTypeBribes(connection)
        PopulateCharacterTypeEnemies(connection)
        PopulateCharacterTypeLoots(connection)
        PopulateCharacterTypePartingShots(connection)
        PopulateDungeonLevels(connection)
        PopulateCharacterTypeSpawnCounts(connection)
        PopulateLocationTypes(connection)
        PopulateCharacterTypeSpawnLocations(connection)
        PopulateEquipSlots(connection)
        PopulateQuestTypes(connection)
        PopulateItemTypeEquipSlots(connection)
        PopulateItemTypeEvents(connection)
        PopulateShoppeTypes(connection)
        PopulateItemTypeShopTypes(connection)
        PopulateItemTypeSpawnCounts(connection)
        PopulateItemTypeSpawnLocationTypes(connection)
        PopulateItemTypeCharacterStatisticBuffs(connection)
        PopulateItemTypeStatisticTypes(connection)
        PopulateItemTypeStatistics(connection)
        PopulateFeatureTypes(connection)
        PopulateDirections(connection)
        PopulateRouteTypes(connection)
        PopulateRouteTypeLocks(connection)
        PopulateSpellTypes(connection)
        PopulateSpellTypeRequiredPowers(connection)
        PopulateLocations(connection)
        PopulateCharacters(connection)
        PopulateCharacterLocations(connection)
        PopulateCharacterSpells(connection)
        PopulatePlayers(connection)
        PopulateItems(connection)
        PopulateCharacterEquipSlots(connection)
        PopulateCharacterQuestCompletions(connection)
        PopulateCharacterQuests(connection)
        PopulateCharacterStatistics(connection)
        PopulateInventories(connection)
        PopulateInventoryItems(connection)
        PopulateItemStatistics(connection)
        PopulateLocationDungeonLevels(connection)
        PopulateFeatures(connection)
        PopulateLocationStatistics(connection)
        PopulateRoutes(connection)
        PopulateLores(connection)
        PopulateItemLores(connection)
    End Sub
    Public Const ItemKind1 = 1L
    Private Sub PopulateItemKinds(connection As SqliteConnection)
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
