﻿Public Module CharacterStatisticTypePopulator
    Friend Const CharacterStatisticType1 = 1
    Friend Const CharacterStatisticType2 = 2
    Friend Const CharacterStatisticType3 = 3
    Friend Const CharacterStatisticType4 = 4
    Friend Const CharacterStatisticType5 = 5
    Friend Const CharacterStatisticType6 = 6
    Friend Const CharacterStatisticType7 = 7
    Friend Const CharacterStatisticType8 = 8
    Friend Const CharacterStatisticType9 = 9
    Friend Const CharacterStatisticType10 = 10
    Friend Const CharacterStatisticType11 = 11
    Friend Const CharacterStatisticType12 = 12
    Friend Const CharacterStatisticType13 = 13
    Friend Const CharacterStatisticType14 = 14
    Friend Const CharacterStatisticType15 = 15
    Friend Const CharacterStatisticType16 = 16
    Friend Const CharacterStatisticType17 = 17
    Friend Const CharacterStatisticType18 = 18
    Friend Const CharacterStatisticType19 = 19
    Friend Const CharacterStatisticType20 = 20
    Friend Const CharacterStatisticType21 = 21
    Friend Const CharacterStatisticType22 = 22
    Friend Const CharacterStatisticType23 = 23
    Friend Const CharacterStatisticType24 = 24
    Friend Const CharacterStatisticType25 = 25
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
    End Sub
End Module