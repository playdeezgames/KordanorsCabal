Public Module StatisticTypePopulator
    Public Const CharacterStatisticType1 = 1
    Public Const CharacterStatisticType2 = 2
    Public Const CharacterStatisticType3 = 3
    Public Const CharacterStatisticType4 = 4
    Public Const CharacterStatisticType5 = 5
    Public Const CharacterStatisticType6 = 6
    Public Const CharacterStatisticType7 = 7
    Public Const CharacterStatisticType8 = 8
    Public Const CharacterStatisticType9 = 9
    Public Const CharacterStatisticType10 = 10
    Public Const CharacterStatisticType11 = 11
    Public Const CharacterStatisticType12 = 12
    Public Const CharacterStatisticType13 = 13
    Public Const CharacterStatisticType14 = 14
    Public Const CharacterStatisticType15 = 15
    Public Const CharacterStatisticType16 = 16
    Public Const CharacterStatisticType17 = 17
    Public Const CharacterStatisticType18 = 18
    Public Const CharacterStatisticType19 = 19
    Public Const CharacterStatisticType20 = 20
    Public Const CharacterStatisticType21 = 21
    Public Const CharacterStatisticType22 = 22
    Public Const CharacterStatisticType23 = 23
    Public Const CharacterStatisticType24 = 24
    Public Const CharacterStatisticType25 = 25
    Public Const StatisticTypeDungeonColumn = 26
    Public Const StatisticTypeDungeonRow = 27
    Public Const ItemStatisticType28 = 28L
    Public Const ItemTypeStatisticType29 = 29L
    Public Const ItemTypeStatisticType30 = 30L
    Public Const ItemTypeStatisticType31 = 31L
    Public Const ItemTypeStatisticType32 = 32L
    Public Const ItemTypeStatisticType33 = 33L
    Public Const ItemTypeStatisticType34 = 34L
    Public Const ItemTypeStatisticType35 = 35L
    Public Const ItemTypeStatisticType36 = 36L
    Private Sub PopulateCharacterStatisticTypesRecord(connection As SqliteConnection, CharacterStatisticTypeId As Long, CharacterStatisticTypeName As String, Abbreviation As String, MinimumValue As Long, DefaultValue As Long?, MaximumValue As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.CharacterStatisticTypes}]([{Columns.CharacterStatisticTypeIdColumn}], [{Columns.CharacterStatisticTypeNameColumn}], [{Columns.AbbreviationColumn}], [{Columns.MinimumValueColumn}], [{Columns.DefaultValueColumn}], [{Columns.MaximumValueColumn}]) VALUES (@{Columns.CharacterStatisticTypeIdColumn}, @{Columns.CharacterStatisticTypeNameColumn}, @{Columns.AbbreviationColumn}, @{Columns.MinimumValueColumn}, @{Columns.DefaultValueColumn}, @{Columns.MaximumValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.CharacterStatisticTypeIdColumn}", CharacterStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.CharacterStatisticTypeNameColumn}", CharacterStatisticTypeName)
            command.Parameters.AddWithValue($"@{Columns.AbbreviationColumn}", Abbreviation)
            command.Parameters.AddWithValue($"@{Columns.MinimumValueColumn}", MinimumValue)
            command.Parameters.AddWithValue($"@{Columns.DefaultValueColumn}", If(DefaultValue Is Nothing, CObj(DBNull.Value), DefaultValue))
            command.Parameters.AddWithValue($"@{Columns.MaximumValueColumn}", MaximumValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Sub PopulateCharacterStatisticTypes(connection As SqliteConnection)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType1, "Strength", "STR", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType2, "Dexterity", "DEX", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType3, "Influence", "INF", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType4, "Willpower", "WIL", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType5, "Power", "POW", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType6, "HP", "HP", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType7, "MP", "MP", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType8, "Mana", "Mana", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType9, "Unassigned", "Unassigned", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType10, "Unarmed Maximum Damage", "MAXDMG", 0, 1, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType11, "Base Maximum Defend", "MAXDEF", 0, 1, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType12, "Wounds", "Wounds", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType13, "Stress", "Stress", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType14, "Money", "$", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType15, "Fatigue", "Fatigue", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType16, "XP", "XP", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType17, "XP Goal", "XP Goal", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType18, "Drunkenness", "Drunkenness", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType19, "Highness", "Highness", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType20, "Hunger", "Hunger", 0, 0, 100)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType21, "Food Poisoning", "Food Poisoning", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType22, "Chafing", "Chafing", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType23, "Immobilization", "Immobilization", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType24, "Base Lift", "Base Lift", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, CharacterStatisticType25, "Bonus Lift", "Bonus Lift", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDungeonColumn, "Dungeon Column", "Column", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDungeonRow, "Dungeon Row", "Row", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemStatisticType28, "Durability", "Durability", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType29, "Encumbrance", "Enc", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType30, "AttackDice", "ATK", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType31, "MaximumDamage", "MaxDMG", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType32, "DefendDice", "DEF", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType33, "MaximumDurability", "MaxDUR", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType34, "Offer", "Offer", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType35, "Price", "Price", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, ItemTypeStatisticType36, "RepairPrice", "Repair", 0, Nothing, 99999)
    End Sub
    Private Sub PopulateItemStatisticTypesRecord(connection As SqliteConnection, ItemStatisticTypeId As Long, DefaultValue As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemStatisticTypes}]([{Columns.ItemStatisticTypeIdColumn}], [{Columns.DefaultValueColumn}]) VALUES (@{Columns.ItemStatisticTypeIdColumn}, @{Columns.DefaultValueColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemStatisticTypeIdColumn}", ItemStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.DefaultValueColumn}", DefaultValue)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Sub PopulateItemStatisticTypes(connection As SqliteConnection)
        PopulateItemStatisticTypesRecord(connection, ItemStatisticType28, 0)
    End Sub
    Private Sub PopulateItemTypeStatisticTypesRecord(connection As SqliteConnection, ItemTypeStatisticTypeId As Long, ItemTypeStatisticTypeName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemTypeStatisticTypes}]([{Columns.ItemTypeStatisticTypeIdColumn}], [{Columns.ItemTypeStatisticTypeNameColumn}]) VALUES (@{Columns.ItemTypeStatisticTypeIdColumn}, @{Columns.ItemTypeStatisticTypeNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeStatisticTypeIdColumn}", ItemTypeStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeStatisticTypeNameColumn}", ItemTypeStatisticTypeName)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Sub PopulateItemTypeStatisticTypes(connection As SqliteConnection)
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType29, "Encumbrance")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType30, "AttackDice")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType31, "MaximumDamage")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType32, "DefendDice")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType33, "MaximumDurability")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType34, "Offer")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType35, "Price")
        PopulateItemTypeStatisticTypesRecord(connection, ItemTypeStatisticType36, "RepairPrice")
    End Sub
End Module
