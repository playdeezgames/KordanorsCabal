Module SpellTypePopulator
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
    Public Const SpellType1 = 1L
    Public Const SpellType2 = 2L
    Sub PopulateSpellTypes(connection As SqliteConnection)
        PopulateSpellTypesRecord(connection, SpellType1, "Holy Bolt", 1, "CharacterCanCastHolyBolt", "CharacterCastHolyBolt")
        PopulateSpellTypesRecord(connection, SpellType2, "Purify", 1, "CharacterCanCastPurify", "CharacterCastPurify")
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
        PopulateSpellTypeRequiredPowersRecord(connection, SpellType1, 0, 0)
        PopulateSpellTypeRequiredPowersRecord(connection, SpellType1, 1, 1)
        PopulateSpellTypeRequiredPowersRecord(connection, SpellType2, 0, 0)
        PopulateSpellTypeRequiredPowersRecord(connection, SpellType2, 1, 0)
    End Sub
End Module
