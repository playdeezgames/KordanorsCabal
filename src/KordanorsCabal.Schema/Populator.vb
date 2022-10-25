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
End Module
