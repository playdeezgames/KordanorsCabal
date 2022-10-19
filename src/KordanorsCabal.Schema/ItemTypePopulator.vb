Friend Module ItemTypePopulator
    Friend Const ItemType1 = 1
    Friend Const ItemType2 = 2
    Friend Const ItemType3 = 3
    Friend Const ItemType4 = 4
    Friend Const ItemType5 = 5
    Friend Const ItemType6 = 6
    Friend Const ItemType7 = 7
    Friend Const ItemType8 = 8
    Friend Const ItemType9 = 9
    Friend Const ItemType10 = 10
    Friend Const ItemType11 = 11
    Friend Const ItemType12 = 12
    Friend Const ItemType13 = 13
    Friend Const ItemType14 = 14
    Friend Const ItemType15 = 15
    Friend Const ItemType16 = 16
    Friend Const ItemType17 = 17
    Friend Const ItemType18 = 18
    Friend Const ItemType19 = 19
    Friend Const ItemType20 = 20
    Friend Const ItemType21 = 21
    Friend Const ItemType22 = 22
    Friend Const ItemType23 = 23
    Friend Const ItemType24 = 24
    Friend Const ItemType25 = 25
    Friend Const ItemType26 = 26
    Friend Const ItemType27 = 27
    Friend Const ItemType28 = 28
    Friend Const ItemType29 = 29
    Friend Const ItemType30 = 30
    Friend Const ItemType31 = 31
    Friend Const ItemType32 = 32
    Friend Const ItemType33 = 33
    Friend Const ItemType34 = 34
    Friend Const ItemType35 = 35
    Friend Const ItemType36 = 36
    Friend Const ItemType37 = 37
    Friend Const ItemType38 = 38
    Friend Const ItemType39 = 39
    Friend Const ItemType40 = 40
    Friend Const ItemType41 = 41
    Friend Const ItemType42 = 42
    Friend Const ItemType43 = 43
    Friend Const ItemType44 = 44
    Friend Const ItemType45 = 45
    Friend Const ItemType46 = 46
    Friend Const ItemType47 = 47
    Friend Const ItemType48 = 48
    Friend Const ItemType49 = 49
    Friend Const ItemType50 = 50
    Friend Const ItemType51 = 51
    Friend Const ItemType52 = 52
    Private Sub PopulateItemTypesRecord(connection As SqliteConnection, ItemTypeId As Long, ItemTypeName As String, IsConsumed As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}], [{Columns.ItemTypeNameColumn}], [{Columns.IsConsumedColumn}]) VALUES (@{Columns.ItemTypeIdColumn}, @{Columns.ItemTypeNameColumn}, @{Columns.IsConsumedColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeIdColumn}", ItemTypeId)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeNameColumn}", ItemTypeName)
            command.Parameters.AddWithValue($"@{Columns.IsConsumedColumn}", IsConsumed)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Sub PopulateItemTypes(connection As SqliteConnection)
        PopulateItemTypesRecord(connection, ItemType1, "FE Key", 1)
        PopulateItemTypesRecord(connection, ItemType2, "CU Key", 1)
        PopulateItemTypesRecord(connection, ItemType3, "AG Key", 1)
        PopulateItemTypesRecord(connection, ItemType4, "AU Key", 1)
        PopulateItemTypesRecord(connection, ItemType5, "PT Key", 1)
        PopulateItemTypesRecord(connection, ItemType6, "Elemental Orb", 0)
        PopulateItemTypesRecord(connection, ItemType7, "Potion", 1)
        PopulateItemTypesRecord(connection, ItemType8, "Goblin Ear", 1)
        PopulateItemTypesRecord(connection, ItemType9, "Skull Fragment", 1)
        PopulateItemTypesRecord(connection, ItemType10, "Dagger", 1)
        PopulateItemTypesRecord(connection, ItemType11, "Earth Shard", 0)
        PopulateItemTypesRecord(connection, ItemType12, "Water Shard", 0)
        PopulateItemTypesRecord(connection, ItemType13, "Fire Shard", 0)
        PopulateItemTypesRecord(connection, ItemType14, "Air Shard", 0)
        PopulateItemTypesRecord(connection, ItemType15, "Shield", 1)
        PopulateItemTypesRecord(connection, ItemType16, "Helmet", 1)
        PopulateItemTypesRecord(connection, ItemType17, "Chainmail", 1)
        PopulateItemTypesRecord(connection, ItemType18, "Shortsword", 1)
        PopulateItemTypesRecord(connection, ItemType19, "Brodesode", 1)
        PopulateItemTypesRecord(connection, ItemType20, "Platemail", 1)
        PopulateItemTypesRecord(connection, ItemType21, "Rat Tail", 1)
        PopulateItemTypesRecord(connection, ItemType22, "Holy ""Water""", 1)
        PopulateItemTypesRecord(connection, ItemType23, "Town Portal", 1)
        PopulateItemTypesRecord(connection, ItemType24, "Food", 1)
        PopulateItemTypesRecord(connection, ItemType25, "Magic Egg", 1)
        PopulateItemTypesRecord(connection, ItemType26, "Beer", 1)
        PopulateItemTypesRecord(connection, ItemType27, "Trousers", 1)
        PopulateItemTypesRecord(connection, ItemType28, "Pr0n Scroll", 1)
        PopulateItemTypesRecord(connection, ItemType29, "Moon Portal", 1)
        PopulateItemTypesRecord(connection, ItemType30, "Empty Bottle", 1)
        PopulateItemTypesRecord(connection, ItemType31, "Book of Holy Bolt", 1)
        PopulateItemTypesRecord(connection, ItemType32, "Membership Card", 1)
        PopulateItemTypesRecord(connection, ItemType33, "Bong", 1)
        PopulateItemTypesRecord(connection, ItemType34, """Herb""", 1)
        PopulateItemTypesRecord(connection, ItemType35, "Food", 1)
        PopulateItemTypesRecord(connection, ItemType36, "Mushroom", 1)
        PopulateItemTypesRecord(connection, ItemType37, "Rotten Egg", 1)
        PopulateItemTypesRecord(connection, ItemType38, "Zombie Taint", 1)
        PopulateItemTypesRecord(connection, ItemType39, "Lotion", 1)
        PopulateItemTypesRecord(connection, ItemType40, "Bat Wing", 1)
        PopulateItemTypesRecord(connection, ItemType41, "Snake Fang", 1)
        PopulateItemTypesRecord(connection, ItemType42, "Shoe Laces", 1)
        PopulateItemTypesRecord(connection, ItemType43, "Spacesord", 1)
        PopulateItemTypesRecord(connection, ItemType44, "Horns of Kordanor", 1)
        PopulateItemTypesRecord(connection, ItemType45, "Amulet of HP", 1)
        PopulateItemTypesRecord(connection, ItemType46, "Ring of HP", 1)
        PopulateItemTypesRecord(connection, ItemType47, "Book of Purify", 1)
        PopulateItemTypesRecord(connection, ItemType48, "Amulet of STR", 1)
        PopulateItemTypesRecord(connection, ItemType49, "Amulet of DEX", 1)
        PopulateItemTypesRecord(connection, ItemType50, "Amulet of POW", 1)
        PopulateItemTypesRecord(connection, ItemType51, "Amulet of Mana", 1)
        PopulateItemTypesRecord(connection, ItemType52, "Amulet of Yendor", 1)
    End Sub
    Sub PopulateItemTypeShopTypesRecord(connection As SqliteConnection, ItemTypeId As Long, ShoppeTypeId As Long, TransactionTypeId As Long)
        Using command = New SqliteCommand($"INSERT INTO [{Tables.ItemTypeShopTypes}]([{Columns.ItemTypeIdColumn}], [{Columns.ShoppeTypeIdColumn}], [{Columns.TransactionTypeIdColumn}]) VALUES (@{Columns.ItemTypeIdColumn}, @{Columns.ShoppeTypeIdColumn}, @{Columns.TransactionTypeIdColumn});", connection)
            command.Parameters.AddWithValue($"@{Columns.ItemTypeIdColumn}", ItemTypeId)
            command.Parameters.AddWithValue($"@{Columns.ShoppeTypeIdColumn}", ShoppeTypeId)
            command.Parameters.AddWithValue($"@{Columns.TransactionTypeIdColumn}", TransactionTypeId)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Friend Const TransactionTypeOffer = 1L
    Friend Const TransactionTypePrice = 2L
    Friend Const TransactionTypeRepair = 3L
    Sub PopulateItemTypeShopTypes(connection As SqliteConnection)
        PopulateItemTypeShopTypesRecord(connection, ItemType7, ShoppeType4, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType8, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType9, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType10, ShoppeType2, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType10, ShoppeType2, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType10, ShoppeType2, TransactionTypeRepair)
        PopulateItemTypeShopTypesRecord(connection, ItemType15, ShoppeType2, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType15, ShoppeType2, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType15, ShoppeType2, TransactionTypeRepair)
        PopulateItemTypeShopTypesRecord(connection, ItemType16, ShoppeType2, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType16, ShoppeType2, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType16, ShoppeType2, TransactionTypeRepair)
        PopulateItemTypeShopTypesRecord(connection, ItemType17, ShoppeType2, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType17, ShoppeType2, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType17, ShoppeType2, TransactionTypeRepair)
        PopulateItemTypeShopTypesRecord(connection, ItemType18, ShoppeType2, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType18, ShoppeType2, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType18, ShoppeType2, TransactionTypeRepair)
        PopulateItemTypeShopTypesRecord(connection, ItemType19, ShoppeType2, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType19, ShoppeType2, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType29, ShoppeType2, TransactionTypeRepair)
        PopulateItemTypeShopTypesRecord(connection, ItemType20, ShoppeType2, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType20, ShoppeType2, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType20, ShoppeType2, TransactionTypeRepair)
        PopulateItemTypeShopTypesRecord(connection, ItemType21, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType22, ShoppeType4, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType23, ShoppeType1, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType24, ShoppeType3, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType25, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType26, ShoppeType3, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType27, ShoppeType5, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType28, ShoppeType5, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType29, ShoppeType1, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType31, ShoppeType1, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType33, ShoppeType1, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType34, ShoppeType1, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType36, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType38, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType39, ShoppeType5, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType40, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType41, ShoppeType1, TransactionTypeOffer)
        PopulateItemTypeShopTypesRecord(connection, ItemType47, ShoppeType1, TransactionTypePrice)
        PopulateItemTypeShopTypesRecord(connection, ItemType52, ShoppeType5, TransactionTypePrice)
    End Sub
End Module
