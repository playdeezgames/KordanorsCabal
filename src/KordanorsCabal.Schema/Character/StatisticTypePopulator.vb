Public Module StatisticTypePopulator
    Public Const StatisticTypeStrength = 1
    Public Const StatisticTypeDexterity = 2
    Public Const StatisticTypeInfluence = 3
    Public Const StatisticTypeWillpower = 4
    Public Const StatisticTypePower = 5
    Public Const StatisticTypeHP = 6
    Public Const StatisticTypeMP = 7
    Public Const StatisticTypeMana = 8
    Public Const StatisticTypeUnassigned = 9
    Public Const StatisticTypeUnarmedMaximumDamage = 10
    Public Const StatisticTypeMaximumDefend = 11
    Public Const StatisticTypeWounds = 12
    Public Const StatisticTypeStress = 13
    Public Const StatisticTypeMoney = 14
    Public Const StatisticTypeFatigue = 15
    Public Const StatisticTypeXP = 16
    Public Const StatisticTypeXPGoal = 17
    Public Const StatisticTypeDrunkenness = 18
    Public Const StatisticTypeHighness = 19
    Public Const StatisticTypeHunger = 20
    Public Const StatisticTypeFoodPoisoning = 21
    Public Const StatisticTypeChafing = 22
    Public Const StatisticTypeImmobilization = 23
    Public Const StatisticTypeBaseLift = 24
    Public Const StatisticTypeBonusLift = 25
    Public Const StatisticTypeDungeonColumn = 26
    Public Const StatisticTypeDungeonRow = 27
    Public Const StatisticTypeDurability = 28L
    Public Const StatisticTypeEncumbrance = 29L
    Public Const StatisticTypeAttackDice = 30L
    Public Const StatisticTypeMaximumDamage = 31L
    Public Const StatisticTypeDefendDice = 32L
    Public Const StatisticTypeMaximumDurability = 33L
    Public Const StatisticTypeOffer = 34L
    Public Const StatisticTypePrice = 35L
    Public Const StatisticTypeRepairPrice = 36L
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
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeStrength, "Strength", "STR", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDexterity, "Dexterity", "DEX", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeInfluence, "Influence", "INF", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeWillpower, "Willpower", "WIL", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypePower, "Power", "POW", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeHP, "HP", "HP", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeMP, "MP", "MP", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeMana, "Mana", "Mana", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeUnassigned, "Unassigned", "Unassigned", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeUnarmedMaximumDamage, "Unarmed Maximum Damage", "MAXDMG", 0, 1, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeMaximumDefend, "Base Maximum Defend", "MAXDEF", 0, 1, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeWounds, "Wounds", "Wounds", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeStress, "Stress", "Stress", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeMoney, "Money", "$", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeFatigue, "Fatigue", "Fatigue", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeXP, "XP", "XP", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeXPGoal, "XP Goal", "XP Goal", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDrunkenness, "Drunkenness", "Drunkenness", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeHighness, "Highness", "Highness", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeHunger, "Hunger", "Hunger", 0, 0, 100)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeFoodPoisoning, "Food Poisoning", "Food Poisoning", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeChafing, "Chafing", "Chafing", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeImmobilization, "Immobilization", "Immobilization", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeBaseLift, "Base Lift", "Base Lift", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeBonusLift, "Bonus Lift", "Bonus Lift", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDungeonColumn, "Dungeon Column", "Column", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDungeonRow, "Dungeon Row", "Row", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDurability, "Durability", "Durability", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeEncumbrance, "Encumbrance", "Enc", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeAttackDice, "AttackDice", "ATK", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeMaximumDamage, "MaximumDamage", "MaxDMG", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeDefendDice, "DefendDice", "DEF", 0, 0, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeMaximumDurability, "MaximumDurability", "MaxDUR", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeOffer, "Offer", "Offer", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypePrice, "Price", "Price", 0, Nothing, 99999)
        PopulateCharacterStatisticTypesRecord(connection, StatisticTypeRepairPrice, "RepairPrice", "Repair", 0, Nothing, 99999)
    End Sub
    Private Sub PopulateItemTypeStatisticTypesRecord(connection As SqliteConnection, ItemTypeStatisticTypeId As Long, ItemTypeStatisticTypeName As String)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemTypeStatisticTypes}]([{Columns.ItemTypeStatisticTypeIdColumn}], [{Columns.ItemTypeStatisticTypeNameColumn}]) VALUES (@{Columns.ItemTypeStatisticTypeIdColumn}, @{Columns.ItemTypeStatisticTypeNameColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeStatisticTypeIdColumn}", ItemTypeStatisticTypeId)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeStatisticTypeNameColumn}", ItemTypeStatisticTypeName)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Sub PopulateItemTypeStatisticTypes(connection As SqliteConnection)
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypeEncumbrance, "Encumbrance")
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypeAttackDice, "AttackDice")
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypeMaximumDamage, "MaximumDamage")
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypeDefendDice, "DefendDice")
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypeMaximumDurability, "MaximumDurability")
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypeOffer, "Offer")
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypePrice, "Price")
        PopulateItemTypeStatisticTypesRecord(connection, StatisticTypeRepairPrice, "RepairPrice")
    End Sub
End Module
