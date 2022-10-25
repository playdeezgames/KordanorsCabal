Module CharacterPopulator
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
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterStatistics}]([{Columns.CharacterIdColumn}], [{Columns.StatisticTypeIdColumn}], [{Columns.StatisticValueColumn}]) VALUES (@{Columns.CharacterIdColumn}, @{Columns.StatisticTypeIdColumn}, @{Columns.StatisticValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterIdColumn}", CharacterId)
            command.Parameters.AddWithValue($"@{Columns.StatisticTypeIdColumn}", CharacterStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.StatisticValueColumn}", StatisticValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Sub PopulateCharacterStatistics(connection As SqliteConnection)
    End Sub
End Module
