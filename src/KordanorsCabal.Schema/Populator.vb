Public Module Populator
    Sub Populate(connection As SqliteConnection)
        PopulateCharacterTypes(connection)
        PopulateCharacterStatisticTypes(connection)
        PopulateCharacterTypeInitialStatistics(connection)
        PopulateCharacterTypeAttackTypes(connection)
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
        PopulateItemStatisticTypes(connection)
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

    Private Sub PopulateItemLores(connection As SqliteConnection)
    End Sub
    Friend Const LoreType1 = 1L
    Friend Const LoreType2 = 2L
    Friend Const LoreType3 = 3L
    Friend Const LoreType4 = 4L
    Friend Const LoreType5 = 5L
    Friend Const LoreType6 = 6L
    Friend Const LoreType7 = 7L
    Friend Const LoreType8 = 8L
    Friend Const LoreType9 = 9L
    Friend Const LoreType10 = 10L
    Friend Const LoreType11 = 11L
    Friend Const LoreType12 = 12L
    Friend Const LoreType13 = 13L
    Friend Const LoreType14 = 14L
    Friend Const LoreType15 = 15L
    Friend Const LoreType16 = 16L
    Friend Const LoreType17 = 17L
    Friend Const LoreType18 = 18L
    Friend Const LoreType19 = 19L
    Friend Const LoreType20 = 20L
    Friend Const LoreType21 = 21L
    Friend Const LoreType22 = 22L
    Friend Const LoreType23 = 23L
    Friend Const LoreType24 = 24L
    Friend Const LoreType25 = 25L
    Private Sub PopulateLores(connection As SqliteConnection)
        PopulateLoreRecord(connection, LoreType1, "Lore #1", "Lore #1")
        PopulateLoreRecord(connection, LoreType2, "Lore #2", "Lore #2")
        PopulateLoreRecord(connection, LoreType3, "Lore #3", "Lore #3")
        PopulateLoreRecord(connection, LoreType4, "Lore #4", "Lore #4")
        PopulateLoreRecord(connection, LoreType5, "Lore #5", "Lore #5")
        PopulateLoreRecord(connection, LoreType6, "Lore #6", "Lore #6")
        PopulateLoreRecord(connection, LoreType7, "Lore #7", "Lore #7")
        PopulateLoreRecord(connection, LoreType8, "Lore #8", "Lore #8")
        PopulateLoreRecord(connection, LoreType9, "Lore #9", "Lore #9")
        PopulateLoreRecord(connection, LoreType10, "Lore #10", "Lore #10")
        PopulateLoreRecord(connection, LoreType11, "Lore #11", "Lore #11")
        PopulateLoreRecord(connection, LoreType12, "Lore #12", "Lore #12")
        PopulateLoreRecord(connection, LoreType13, "Lore #13", "Lore #13")
        PopulateLoreRecord(connection, LoreType14, "Lore #14", "Lore #14")
        PopulateLoreRecord(connection, LoreType15, "Lore #15", "Lore #15")
        PopulateLoreRecord(connection, LoreType16, "Lore #16", "Lore #16")
        PopulateLoreRecord(connection, LoreType17, "Lore #17", "Lore #17")
        PopulateLoreRecord(connection, LoreType18, "Lore #18", "Lore #18")
        PopulateLoreRecord(connection, LoreType19, "Lore #19", "Lore #19")
        PopulateLoreRecord(connection, LoreType20, "Lore #20", "Lore #20")
        PopulateLoreRecord(connection, LoreType21, "Lore #21", "Lore #21")
        PopulateLoreRecord(connection, LoreType22, "Lore #22", "Lore #22")
        PopulateLoreRecord(connection, LoreType23, "Lore #23", "Lore #23")
        PopulateLoreRecord(connection, LoreType24, "Lore #24", "Lore #24")
        PopulateLoreRecord(connection, LoreType25, "Lore #25", "Lore #25")
    End Sub

    Private Sub PopulateLoreRecord(connection As SqliteConnection, loreId As Long, itemName As String, loreText As String)
        Using command = New SqliteCommand(
            $"INSERT INTO [{Tables.Lores}]
            (
                [{Columns.LoreIdColumn}], 
                [{Columns.ItemNameColumn}],
                [{Columns.LoreTextColumn}]
            ) 
            VALUES 
            (
                @{Columns.LoreIdColumn}, 
                @{Columns.ItemNameColumn}, 
                @{Columns.LoreTextColumn}
            );", connection)
            command.Parameters.AddWithValue($"@{Columns.LoreIdColumn}", loreId)
            command.Parameters.AddWithValue($"@{Columns.LoreTextColumn}", loreText)
            command.Parameters.AddWithValue($"@{Columns.ItemNameColumn}", itemName)
            command.ExecuteNonQuery()
        End Using
    End Sub

    Sub PopulateRouteTypesRecord(connection As SqliteConnection, RouteTypeId As Long, Abbreviation As String, IsSingleUse As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.RouteTypes}]([{Columns.RouteTypeIdColumn}], [{Columns.AbbreviationColumn}], [{Columns.IsSingleUseColumn}]) VALUES (@{Columns.RouteTypeIdColumn}, @{Columns.AbbreviationColumn}, @{Columns.IsSingleUseColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.RouteTypeIdColumn}", RouteTypeId)
            command.Parameters.AddWithValue($"@{Columns.AbbreviationColumn}", Abbreviation)
            command.Parameters.AddWithValue($"@{Columns.IsSingleUseColumn}", IsSingleUse)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateRouteTypes(connection As SqliteConnection)
        PopulateRouteTypesRecord(connection, 1, "  ", 0)
        PopulateRouteTypesRecord(connection, 2, "  ", 0)
        PopulateRouteTypesRecord(connection, 3, "  ", 0)
        PopulateRouteTypesRecord(connection, 4, "FE", 0)
        PopulateRouteTypesRecord(connection, 5, "CU", 0)
        PopulateRouteTypesRecord(connection, 6, "AG", 0)
        PopulateRouteTypesRecord(connection, 7, "AU", 0)
        PopulateRouteTypesRecord(connection, 8, "PT", 0)
        PopulateRouteTypesRecord(connection, 9, "EO", 0)
        PopulateRouteTypesRecord(connection, 10, "  ", 1)
        PopulateRouteTypesRecord(connection, 11, "  ", 0)
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
        PopulateRouteTypeLocksRecord(connection, 4, 2, 1)
        PopulateRouteTypeLocksRecord(connection, 5, 2, 2)
        PopulateRouteTypeLocksRecord(connection, 6, 2, 3)
        PopulateRouteTypeLocksRecord(connection, 7, 2, 4)
        PopulateRouteTypeLocksRecord(connection, 8, 2, 5)
        PopulateRouteTypeLocksRecord(connection, 9, 2, 6)
    End Sub
    Sub PopulateSpellTypesRecord(connection As SqliteConnection, SpellTypeId As Long, SpellTypeName As String, MaximumLevel As Long, CastCheck As String, Cast As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.SpellTypes}]([{Columns.SpellTypeIdColumn}], [{Columns.SpellTypeNameColumn}], [{Columns.MaximumLevelColumn}], [{Columns.CastCheckColumn}], [{Columns.CastColumn}]) VALUES (@{Columns.SpellTypeIdColumn}, @{Columns.SpellTypeNameColumn}, @{Columns.MaximumLevelColumn}, @{Columns.CastCheckColumn}, @{Columns.CastColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.SpellTypeIdColumn}", SpellTypeId)
            command.Parameters.AddWithValue($"@{Columns.SpellTypeNameColumn}", SpellTypeName)
            command.Parameters.AddWithValue($"@{Columns.MaximumLevelColumn}", MaximumLevel)
            command.Parameters.AddWithValue($"@{Columns.CastCheckColumn}", CastCheck)
            command.Parameters.AddWithValue($"@{Columns.CastColumn}", Cast)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateSpellTypes(connection As SqliteConnection)
        PopulateSpellTypesRecord(connection, 1, "Holy Bolt", 1, "CharacterCanCastHolyBolt", "CharacterCastHolyBolt")
        PopulateSpellTypesRecord(connection, 2, "Purify", 1, "CharacterCanCastPurify", "CharacterCastPurify")
    End Sub
    Sub PopulateSpellTypeRequiredPowersRecord(connection As SqliteConnection, SpellTypeId As Long, SpellLevel As Long, Power As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.SpellTypeRequiredPowers}]([{Columns.SpellTypeIdColumn}], [{Columns.SpellLevelColumn}], [{Columns.PowerColumn}]) VALUES (@{Columns.SpellTypeIdColumn}, @{Columns.SpellLevelColumn}, @{Columns.PowerColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.SpellTypeIdColumn}", SpellTypeId)
            command.Parameters.AddWithValue($"@{Columns.SpellLevelColumn}", SpellLevel)
            command.Parameters.AddWithValue($"@{Columns.PowerColumn}", Power)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateSpellTypeRequiredPowers(connection As SqliteConnection)
        PopulateSpellTypeRequiredPowersRecord(connection, 1, 0, 0)
        PopulateSpellTypeRequiredPowersRecord(connection, 1, 1, 1)
        PopulateSpellTypeRequiredPowersRecord(connection, 2, 0, 0)
        PopulateSpellTypeRequiredPowersRecord(connection, 2, 1, 0)
    End Sub
    Sub PopulateLocationsRecord(connection As SqliteConnection, LocationId As Long, LocationTypeId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Locations}]([{Columns.LocationIdColumn}], [{Columns.LocationTypeIdColumn}]) VALUES (@{Columns.LocationIdColumn}, @{Columns.LocationTypeIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.LocationTypeIdColumn}", LocationTypeId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateLocations(connection As SqliteConnection)
    End Sub
    Sub PopulateCharactersRecord(connection As SqliteConnection, CharacterId As Long, LocationId As Long, CharacterTypeId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Characters}]([{Columns.CharacterIdColumn}], [{Columns.LocationIdColumn}], [{Columns.CharacterTypeIdColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.LocationIdColumn}, @{Columns.CharacterTypeIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.CharacterTypeIdColumn}", CharacterTypeId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacters(connection As SqliteConnection)
    End Sub
    Sub PopulateCharacterLocationsRecord(connection As SqliteConnection, CharacterId As Long, LocationId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterLocations}]([{Columns.CharacterIdColumn}], [{Columns.LocationIdColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.LocationIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacterLocations(connection As SqliteConnection)
    End Sub
    Sub PopulateCharacterSpellsRecord(connection As SqliteConnection, CharacterId As Long, SpellTypeId As Long, SpellLevel As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterSpells}]([{Columns.CharacterIdColumn}], [{Columns.SpellTypeIdColumn}], [{Columns.SpellLevelColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.SpellTypeIdColumn}, @{Columns.SpellLevelColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.SpellTypeIdColumn}", SpellTypeId)
            command.Parameters.AddWithValue($"@{Columns.SpellLevelColumn}", SpellLevel)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacterSpells(connection As SqliteConnection)
    End Sub
    Sub PopulatePlayersRecord(connection As SqliteConnection, PlayerId As Long, CharacterId As Long, DirectionId As Long, PlayerModeId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Players}]([{Columns.PlayerIdColumn}], [{Columns.CharacterIdColumn}], [{Columns.DirectionIdColumn}], [{Columns.PlayerModeIdColumn}]) VALUES (@{Columns.PlayerIdColumn}, @{Columns.CharacterIdColumn}, @{Columns.DirectionIdColumn}, @{Columns.PlayerModeIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.PlayerIdColumn}", PlayerId)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.DirectionIdColumn}", DirectionId)
            command.Parameters.AddWithValue($"@{Columns.PlayerModeIdColumn}", PlayerModeId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulatePlayers(connection As SqliteConnection)
    End Sub
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
    Sub PopulateCharacterEquipSlotsRecord(connection As SqliteConnection, CharacterId As Long, EquipSlotId As Long, ItemId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterEquipSlots}]([{Columns.CharacterIdColumn}], [{Columns.EquipSlotIdColumn}], [{Columns.ItemIdColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.EquipSlotIdColumn}, @{Columns.ItemIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.EquipSlotIdColumn}", EquipSlotId)
            command.Parameters.AddWithValue($"@{Columns.ItemIdColumn}", ItemId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacterEquipSlots(connection As SqliteConnection)
    End Sub
    Sub PopulateCharacterQuestCompletionsRecord(connection As SqliteConnection, CharacterId As Long, QuestTypeId As Long, Completions As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterQuestCompletions}]([{Columns.CharacterIdColumn}], [{Columns.QuestTypeIdColumn}], [{Columns.CompletionsColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.QuestTypeIdColumn}, @{Columns.CompletionsColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.QuestTypeIdColumn}", QuestTypeId)
            command.Parameters.AddWithValue($"@{Columns.CompletionsColumn}", Completions)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacterQuestCompletions(connection As SqliteConnection)
    End Sub
    Sub PopulateCharacterQuestsRecord(connection As SqliteConnection, CharacterId As Long, QuestTypeId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterQuests}]([{Columns.CharacterIdColumn}], [{Columns.QuestTypeIdColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.QuestTypeIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.QuestTypeIdColumn}", QuestTypeId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacterQuests(connection As SqliteConnection)
    End Sub
    Sub PopulateCharacterStatisticsRecord(connection As SqliteConnection, CharacterId As Long, CharacterStatisticTypeId As Long, StatisticValue As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterStatistics}]([{Columns.CharacterIdColumn}], [{Columns.CharacterStatisticTypeIdColumn}], [{Columns.StatisticValueColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.CharacterStatisticTypeIdColumn}, @{Columns.StatisticValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.CharacterStatisticTypeIdColumn}", CharacterStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.StatisticValueColumn}", StatisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacterStatistics(connection As SqliteConnection)
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
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemStatistics}]([{Columns.ItemIdColumn}], [{Columns.ItemStatisticTypeIdColumn}], [{Columns.StatisticValueColumn}]) VALUES (@{Columns.ItemIdColumn}, @{Columns.ItemStatisticTypeIdColumn}, @{Columns.StatisticValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemIdColumn}", ItemId)
            command.Parameters.AddWithValue($"@{Columns.ItemStatisticTypeIdColumn}", ItemStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.StatisticValueColumn}", StatisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateItemStatistics(connection As SqliteConnection)
    End Sub
    Sub PopulateLocationDungeonLevelsRecord(connection As SqliteConnection, LocationId As Long, DungeonLevelId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.LocationDungeonLevels}]([{Columns.LocationIdColumn}], [{Columns.DungeonLevelIdColumn}]) VALUES (@{Columns.LocationIdColumn}, @{Columns.DungeonLevelIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.DungeonLevelIdColumn}", DungeonLevelId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateLocationDungeonLevels(connection As SqliteConnection)
    End Sub
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
    Sub PopulateLocationStatisticsRecord(connection As SqliteConnection, LocationId As Long, LocationStatisticTypeId As Long, StatisticValue As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.LocationStatistics}]([{Columns.LocationIdColumn}], [{Columns.LocationStatisticTypeIdColumn}], [{Columns.StatisticValueColumn}]) VALUES (@{Columns.LocationIdColumn}, @{Columns.LocationStatisticTypeIdColumn}, @{Columns.StatisticValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.LocationStatisticTypeIdColumn}", LocationStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.StatisticValueColumn}", StatisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateLocationStatistics(connection As SqliteConnection)
    End Sub
    Sub PopulateRoutesRecord(connection As SqliteConnection, RouteId As Long, LocationId As Long, DirectionId As Long, RouteTypeId As Long, ToLocationId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.Routes}]([{Columns.RouteIdColumn}], [{Columns.LocationIdColumn}], [{Columns.DirectionIdColumn}], [{Columns.RouteTypeIdColumn}], [{Columns.ToLocationIdColumn}]) VALUES (@{Columns.RouteIdColumn}, @{Columns.LocationIdColumn}, @{Columns.DirectionIdColumn}, @{Columns.RouteTypeIdColumn}, @{Columns.ToLocationIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.RouteIdColumn}", RouteId)
            command.Parameters.AddWithValue($"@{Columns.LocationIdColumn}", LocationId)
            command.Parameters.AddWithValue($"@{Columns.DirectionIdColumn}", DirectionId)
            command.Parameters.AddWithValue($"@{Columns.RouteTypeIdColumn}", RouteTypeId)
            command.Parameters.AddWithValue($"@{Columns.ToLocationIdColumn}", ToLocationId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateRoutes(connection As SqliteConnection)
    End Sub
End Module
