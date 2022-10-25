Public Module Scaffolder
    Private Sub ScaffoldTable(connection As SqliteConnection, sql As String)
        Using command = New SqliteCommand(sql, connection)
            command.ExecuteNonQuery()
        End Using
    End Sub
    Public Sub Scaffold(connection As SqliteConnection)
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypes}]
(
	[{Columns.CharacterTypeIdColumn}] INTEGER PRIMARY KEY,
	[{Columns.CharacterTypeNameColumn}] TEXT NOT NULL,
	[{Columns.XPValueColumn}] INT NOT NULL,
	[{Columns.MoneyDropDiceColumn}] TEXT NOT NULL,
	[{Columns.IsUndeadColumn}] INT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.StatisticTypes}]
(
                    [{Columns.StatisticTypeIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.CharacterStatisticTypeNameColumn}] TEXT NOT NULL,
                    [{Columns.AbbreviationColumn}] TEXT NOT NULL,
                    [{Columns.MinimumValueColumn}] INT NOT NULL,
                    [{Columns.DefaultValueColumn}] INT NULL,
                    [{Columns.MaximumValueColumn}] INT NOT NULL)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypeInitialStatistics}]
(
                    [{Columns.CharacterTypeIdColumn}] INT NOT NULL,
                    [{Columns.StatisticTypeIdColumn}] INT NOT NULL,
                    [{Columns.InitialValueColumn}] INT NOT NULL,
					UNIQUE([{Columns.CharacterTypeIdColumn}],[{Columns.StatisticTypeIdColumn}]),
					FOREIGN KEY ([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}]),
					FOREIGN KEY ([{Columns.StatisticTypeIdColumn}]) REFERENCES [{Tables.StatisticTypes}]([{Columns.StatisticTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypeAttackTypes}]
(
	[{Columns.CharacterTypeIdColumn}] INT NOT NULL,
	[{Columns.AttackTypeColumn}] INT NOT NULL,
	[{Columns.WeightColumn}] INT NOT NULL,
	UNIQUE([{Columns.CharacterTypeIdColumn}],[{Columns.AttackTypeColumn}]),
	FOREIGN KEY ([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemKinds}]
(
	[{Columns.ItemKindIdColumn}] INTEGER PRIMARY KEY,
	[{Columns.ItemKindNameColumn}] TEXT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypes}]
(
	[{Columns.ItemTypeIdColumn}] INTEGER PRIMARY KEY,
	[{Columns.ItemTypeNameColumn}] TEXT NOT NULL,
	[{Columns.IsConsumedColumn}] INT NOT NULL,
    [{Columns.ItemKindIdColumn}] INT NOT NULL,
    FOREIGN KEY ([{Columns.ItemKindIdColumn}]) REFERENCES [{Tables.ItemKinds}]([{Columns.ItemKindIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypeBribes}]
(
                   [{Columns.CharacterTypeIdColumn}] INT NOT NULL,
                    [{Columns.ItemTypeIdColumn}] INT NOT NULL,
					UNIQUE([{Columns.CharacterTypeIdColumn}],[{Columns.ItemTypeIdColumn}]),
					FOREIGN KEY ([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}]),
					FOREIGN KEY ([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypeEnemies}]
(
	[{Columns.CharacterTypeIdColumn}] INT NOT NULL,
    [{Columns.EnemyCharacterTypeIdColumn}] INT NOT NULL,
	UNIQUE([{Columns.CharacterTypeIdColumn}],[{Columns.EnemyCharacterTypeIdColumn}]),
	FOREIGN KEY([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}]),
	FOREIGN KEY([{Columns.EnemyCharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}]),
	CHECK([{Columns.CharacterTypeIdColumn}]<>[{Columns.EnemyCharacterTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypeLoots}]
(
	[{Columns.CharacterTypeIdColumn}] INT NOT NULL,
	[{Columns.ItemTypeIdColumn}] INT NULL,
	[{Columns.WeightColumn}] INT NOT NULL,
	UNIQUE([{Columns.CharacterTypeIdColumn}],[{Columns.ItemTypeIdColumn}]),
	FOREIGN KEY([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}]),
	FOREIGN KEY([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypePartingShots}]
(
                    [{Columns.CharacterTypeIdColumn}] INT NOT NULL,
                    [{Columns.PartingShotColumn}] TEXT NOT NULL,
                    [{Columns.WeightColumn}] INT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.DungeonLevels}]
(
	[{Columns.DungeonLevelIdColumn}] INTEGER PRIMARY KEY,
    [{Columns.DungeonLevelNameColumn}] TEXT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypeSpawnCounts}]
(
	[{Columns.CharacterTypeIdColumn}] INT NOT NULL,
	[{Columns.DungeonLevelIdColumn}] INT NOT NULL,
	[{Columns.SpawnCountColumn}] INT NOT NULL,
	UNIQUE([{Columns.CharacterTypeIdColumn}],[{Columns.DungeonLevelIdColumn}]),
	FOREIGN KEY ([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}]),
	FOREIGN KEY ([{Columns.DungeonLevelIdColumn}]) REFERENCES [{Tables.DungeonLevels}]([{Columns.DungeonLevelIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.LocationTypes}]
(
	[{Columns.LocationTypeIdColumn}] INTEGER PRIMARY KEY,
	[{Columns.LocationTypeNameColumn}] TEXT NOT NULL,
	[{Columns.IsDungeonColumn}] INT NOT NULL,
	[{Columns.CanMapColumn}] INT NOT NULL,
	[{Columns.RequiresMPColumn}] INT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterTypeSpawnLocations}]
(
                    [{Columns.CharacterTypeIdColumn}] INT NOT NULL,
                    [{Columns.DungeonLevelIdColumn}] INT NOT NULL,
                    [{Columns.LocationTypeIdColumn}] INT NOT NULL,
					UNIQUE([{Columns.CharacterTypeIdColumn}],[{Columns.DungeonLevelIdColumn}],[{Columns.LocationTypeIdColumn}]),
					FOREIGN KEY ([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}]),
					FOREIGN KEY ([{Columns.DungeonLevelIdColumn}]) REFERENCES [{Tables.DungeonLevels}]([{Columns.DungeonLevelIdColumn}]),
					FOREIGN KEY ([{Columns.LocationTypeIdColumn}]) REFERENCES [{Tables.LocationTypes}]([{Columns.LocationTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.EquipSlots}]
(
                    [{Columns.EquipSlotIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.EquipSlotNameColumn}] TEXT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.QuestTypes}]
(
                    [{Columns.QuestTypeIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.CanAcceptEventNameColumn}] TEXT NOT NULL,
                    [{Columns.AcceptEventNameColumn}] TEXT NOT NULL,
                    [{Columns.CanCompleteEventNameColumn}] TEXT NOT NULL,
                    [{Columns.CompleteEventNameColumn}] TEXT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeEquipSlots}]
(
                    [{Columns.ItemTypeIdColumn}] INT NOT NULL,
                    [{Columns.EquipSlotIdColumn}] INT NOT NULL,
					UNIQUE([{Columns.ItemTypeIdColumn}],[{Columns.EquipSlotIdColumn}]),
					FOREIGN KEY([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}]),
					FOREIGN KEY([{Columns.EquipSlotIdColumn}]) REFERENCES [{Tables.EquipSlots}]([{Columns.EquipSlotIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeEvents}]
(
	[{Columns.ItemTypeIdColumn}] INT NOT NULL,
	[{Columns.EventIdColumn}] INT NOT NULL,
	[{Columns.EventNameColumn}] TEXT NOT NULL,
	UNIQUE([{Columns.ItemTypeIdColumn}],[{Columns.EventIdColumn}]),
	FOREIGN KEY([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ShoppeTypes}]
(
                    [{Columns.ShoppeTypeIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.ShoppeTypeNameColumn}] TEXT NOT NULL
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeShopTypes}]
(
                    [{Columns.ItemTypeIdColumn}] INT NOT NULL,
                    [{Columns.ShoppeTypeIdColumn}] INT NOT NULL,
                    [{Columns.TransactionTypeIdColumn}] INT NOT NULL,
					UNIQUE([{Columns.ItemTypeIdColumn}],[{Columns.ShoppeTypeIdColumn}],[{Columns.TransactionTypeIdColumn}]),
					FOREIGN KEY ([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}]),
					FOREIGN KEY ([{Columns.ShoppeTypeIdColumn}]) REFERENCES [{Tables.ShoppeTypes}]([{Columns.ShoppeTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeSpawnCounts}]
(
                    [{Columns.ItemTypeIdColumn}] INT NOT NULL,
                    [{Columns.DungeonLevelIdColumn}] INT NOT NULL,
                    [{Columns.SpawnDiceColumn}] TEXT NOT NULL,
					UNIQUE([{Columns.ItemTypeIdColumn}],[{Columns.DungeonLevelIdColumn}]),
					FOREIGN KEY([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}]),
					FOREIGN KEY([{Columns.DungeonLevelIdColumn}]) REFERENCES [{Tables.DungeonLevels}]([{Columns.DungeonLevelIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeSpawnLocationTypes}]
(
                    [{Columns.ItemTypeIdColumn}] INT NOT NULL,
                    [{Columns.DungeonLevelIdColumn}] INT NOT NULL,
                    [{Columns.LocationTypeIdColumn}] INT NOT NULL,
					UNIQUE([{Columns.ItemTypeIdColumn}],[{Columns.DungeonLevelIdColumn}],[{Columns.LocationTypeIdColumn}]),
					FOREIGN KEY ([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}]),
					FOREIGN KEY ([{Columns.DungeonLevelIdColumn}]) REFERENCES [{Tables.DungeonLevels}]([{Columns.DungeonLevelIdColumn}]),
					FOREIGN KEY ([{Columns.LocationTypeIdColumn}]) REFERENCES [{Tables.LocationTypes}]([{Columns.LocationTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeCharacterStatisticBuffs}]
(
                    [{Columns.ItemTypeIdColumn}] INT NOT NULL,
                    [{Columns.StatisticTypeIdColumn}] INT NOT NULL,
                    [{Columns.BuffColumn}] INT NOT NULL,
					UNIQUE([{Columns.ItemTypeIdColumn}],[{Columns.StatisticTypeIdColumn}]),
					FOREIGN KEY ([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}]),
					FOREIGN KEY ([{Columns.StatisticTypeIdColumn}]) REFERENCES [{Tables.StatisticTypes}]([{Columns.StatisticTypeIdColumn}])
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeStatisticTypes}]
(
                    [{Columns.ItemTypeStatisticTypeIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.ItemTypeStatisticTypeNameColumn}] TEXT NOT NULL
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemTypeStatistics}]
(
                    [{Columns.ItemTypeIdColumn}] INT NOT NULL,
                    [{Columns.ItemTypeStatisticTypeIdColumn}] INT NOT NULL,
                    [{Columns.ItemTypeStatisticValueColumn}] INT NOT NULL,
					UNIQUE([{Columns.ItemTypeIdColumn}],[{Columns.ItemTypeStatisticTypeIdColumn}]),
					FOREIGN KEY ([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}]),
					FOREIGN KEY ([{Columns.ItemTypeStatisticTypeIdColumn}]) REFERENCES [{Tables.ItemTypeStatisticTypes}]([{Columns.ItemTypeStatisticTypeIdColumn}])
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.FeatureTypes}]
(
                    [{Columns.FeatureTypeIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.FeatureTypeNameColumn}] TEXT NOT NULL,
                    [{Columns.LocationTypeIdColumn}] INT NOT NULL,
                    [{Columns.InteractionModeColumn}] INT NOT NULL,
					FOREIGN KEY ([{Columns.LocationTypeIdColumn}]) REFERENCES [{Tables.LocationTypes}]([{Columns.LocationTypeIdColumn}])
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Directions}]
(
                    [{Columns.DirectionIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.DirectionNameColumn}] TEXT NOT NULL,
                    [{Columns.AbbreviationColumn}] TEXT NOT NULL,
                    [{Columns.IsCardinalColumn}] INT NOT NULL,
                    [{Columns.PreviousDirectionIdColumn}] INT NULL,
                    [{Columns.OppositeDirectionIdColumn}] INT NOT NULL,
                    [{Columns.NextDirectionIdColumn}] INT NULL
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.RouteTypes}]
(
                    [{Columns.RouteTypeIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.AbbreviationColumn}] TEXT NOT NULL,
                    [{Columns.IsSingleUseColumn}] INT NOT NULL
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.RouteTypeLocks}]
(
	[{Columns.RouteTypeIdColumn}] INT NOT NULL,
	[{Columns.UnlockedRouteTypeIdColumn}] INT NOT NULL,
	[{Columns.UnlockItemTypeIdColumn}] INT NOT NULL,
	UNIQUE([{Columns.RouteTypeIdColumn}]),
	FOREIGN KEY ([{Columns.RouteTypeIdColumn}]) REFERENCES [{Tables.RouteTypes}]([{Columns.RouteTypeIdColumn}]),
	FOREIGN KEY ([{Columns.UnlockedRouteTypeIdColumn}]) REFERENCES [{Tables.RouteTypes}]([{Columns.RouteTypeIdColumn}]),
	FOREIGN KEY ([{Columns.UnlockItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.SpellTypes}]
(
                    [{Columns.SpellTypeIdColumn}] INTEGER PRIMARY KEY,
                    [{Columns.SpellTypeNameColumn}] TEXT NOT NULL,
                    [{Columns.MaximumLevelColumn}] INT NOT NULL,
                    [{Columns.CastCheckColumn}] TEXT NOT NULL,
                    [{Columns.CastColumn}] TEXT NOT NULL
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.SpellTypeRequiredPowers}]
(
                    [{Columns.SpellTypeIdColumn}] INT NOT NULL,
                    [{Columns.SpellLevelColumn}] INT NOT NULL,
                    [{Columns.PowerColumn}] INT NOT NULL,
					UNIQUE([{Columns.SpellTypeIdColumn}],[{Columns.SpellLevelColumn}]),
					FOREIGN KEY([{Columns.SpellTypeIdColumn}]) REFERENCES [{Tables.SpellTypes}]([{Columns.SpellTypeIdColumn}])
					)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Locations}]
(
	[{Columns.LocationIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
	[{Columns.LocationTypeIdColumn}] INT NOT NULL,
	FOREIGN KEY ([{Columns.LocationTypeIdColumn}]) REFERENCES [{Tables.LocationTypes}]([{Columns.LocationTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Characters}]
(
	[{Columns.CharacterIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
	[{Columns.LocationIdColumn}] INT NOT NULL,
	[{Columns.CharacterTypeIdColumn}] INT NOT NULL,
	FOREIGN KEY ([{Columns.LocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}]),
	FOREIGN KEY ([{Columns.CharacterTypeIdColumn}]) REFERENCES [{Tables.CharacterTypes}]([{Columns.CharacterTypeIdColumn}])
)")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterLocations}]
            (
                [{Columns.CharacterIdColumn}] INT NOT NULL,
                [{Columns.LocationIdColumn}] INT NOT NULL,
                UNIQUE([{Columns.CharacterIdColumn}],[{Columns.LocationIdColumn}]),
                FOREIGN KEY([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY([{Columns.LocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterSpells}]
            (
                [{Columns.CharacterIdColumn}] INT NOT NULL,
                [{Columns.SpellTypeIdColumn}] INT NOT NULL,
                [{Columns.SpellLevelColumn}] INT NOT NULL,
                UNIQUE([{Columns.CharacterIdColumn}],[{Columns.SpellTypeIdColumn}]),
                FOREIGN KEY ([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY ([{Columns.SpellTypeIdColumn}]) REFERENCES [{Tables.SpellTypes}]([{Columns.SpellTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Players}]
            (
                [{Columns.PlayerIdColumn}] INT NOT NULL UNIQUE CHECK([{Columns.PlayerIdColumn}]=1),
                [{Columns.CharacterIdColumn}] INT NOT NULL,
                [{Columns.DirectionIdColumn}] INT NOT NULL,
                [{Columns.PlayerModeIdColumn}] INT NOT NULL,
                FOREIGN KEY ([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY ([{Columns.DirectionIdColumn}]) REFERENCES [{Tables.Directions}]([{Columns.DirectionIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Items}]
            (
                [{Columns.ItemIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{Columns.ItemTypeIdColumn}] INT NOT NULL,
                [{Columns.ItemNameColumn}] TEXT NULL,
				FOREIGN KEY ([{Columns.ItemTypeIdColumn}]) REFERENCES [{Tables.ItemTypes}]([{Columns.ItemTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterEquipSlots}]
            (
                [{Columns.CharacterIdColumn}] INT NOT NULL,
                [{Columns.EquipSlotIdColumn}] INT NOT NULL,
                [{Columns.ItemIdColumn}] INT NOT NULL UNIQUE,
                UNIQUE([{Columns.CharacterIdColumn}],[{Columns.EquipSlotIdColumn}]),
                FOREIGN KEY ([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY ([{Columns.ItemIdColumn}]) REFERENCES [{Tables.Items}]([{Columns.ItemIdColumn}]),
                FOREIGN KEY ([{Columns.EquipSlotIdColumn}]) REFERENCES [{Tables.EquipSlots}]([{Columns.EquipSlotIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterQuestCompletions}]
            (
                [{Columns.CharacterIdColumn}] INT NOT NULL,
                [{Columns.QuestTypeIdColumn}] INT NOT NULL,
                [{Columns.CompletionsColumn}] INT NOT NULL,
                UNIQUE([{Columns.CharacterIdColumn}],[{Columns.QuestTypeIdColumn}]),
                FOREIGN KEY([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY([{Columns.QuestTypeIdColumn}]) REFERENCES [{Tables.QuestTypes}]([{Columns.QuestTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterQuests}]
            (
                [{Columns.CharacterIdColumn}] INT NOT NULL,
                [{Columns.QuestTypeIdColumn}] INT NOT NULL,
                UNIQUE([{Columns.CharacterIdColumn}],[{Columns.QuestTypeIdColumn}]),
                FOREIGN KEY([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY([{Columns.QuestTypeIdColumn}]) REFERENCES [{Tables.QuestTypes}]([{Columns.QuestTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.CharacterStatistics}]
            (
                [{Columns.CharacterIdColumn}] INT NOT NULL,
                [{Columns.StatisticTypeIdColumn}] INT NOT NULL,
                [{Columns.StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{Columns.CharacterIdColumn}],[{Columns.StatisticTypeIdColumn}]),
                FOREIGN KEY ([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY ([{Columns.StatisticTypeIdColumn}]) REFERENCES [{Tables.StatisticTypes}]([{Columns.StatisticTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Inventories}]
            (
                [{Columns.InventoryIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{Columns.CharacterIdColumn}] INT NULL UNIQUE,
                [{Columns.LocationIdColumn}] INT NULL UNIQUE,
                FOREIGN KEY ([{Columns.CharacterIdColumn}]) REFERENCES [{Tables.Characters}]([{Columns.CharacterIdColumn}]),
                FOREIGN KEY ([{Columns.LocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}]),
                CHECK(
                    ([{Columns.CharacterIdColumn}] IS NULL AND [{Columns.LocationIdColumn}] IS NOT NULL) OR 
                    ([{Columns.CharacterIdColumn}] IS NOT NULL AND [{Columns.LocationIdColumn}] IS NULL)
                )
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.InventoryItems}]
            (
                [{Columns.InventoryIdColumn}] INT NOT NULL,
                [{Columns.ItemIdColumn}] INT NOT NULL UNIQUE,
				FOREIGN KEY([{Columns.InventoryIdColumn}]) REFERENCES [{Tables.Inventories}]([{Columns.InventoryIdColumn}]),
				FOREIGN KEY([{Columns.ItemIdColumn}]) REFERENCES [{Tables.Items}]([{Columns.ItemIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemStatistics}]
            (
                [{Columns.ItemIdColumn}] INT NOT NULL,
                [{Columns.StatisticTypeIdColumn}] INT NOT NULL,
                [{Columns.StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{Columns.ItemIdColumn}],[{Columns.StatisticTypeIdColumn}]),
                FOREIGN KEY ([{Columns.ItemIdColumn}]) REFERENCES [{Tables.Items}]([{Columns.ItemIdColumn}]),
                FOREIGN KEY ([{Columns.StatisticTypeIdColumn}]) REFERENCES [{Tables.StatisticTypes}]([{Columns.StatisticTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.LocationDungeonLevels}]
            (
                [{Columns.LocationIdColumn}] INT NOT NULL UNIQUE,
                [{Columns.DungeonLevelIdColumn}] INT NOT NULL,
				FOREIGN KEY ([{Columns.LocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}]),
				FOREIGN KEY ([{Columns.DungeonLevelIdColumn}]) REFERENCES [{Tables.DungeonLevels}]([{Columns.DungeonLevelIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Features}]
            (
                [{Columns.FeatureIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{Columns.FeatureTypeIdColumn}] INT NOT NULL,
                [{Columns.LocationIdColumn}] INT NOT NULL UNIQUE,
                FOREIGN KEY([{Columns.LocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}]),
                FOREIGN KEY([{Columns.FeatureTypeIdColumn}]) REFERENCES [{Tables.FeatureTypes}]([{Columns.FeatureTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.LocationStatistics}]
            (
                [{Columns.LocationIdColumn}] INT NOT NULL,
                [{Columns.StatisticTypeIdColumn}] INT NOT NULL, 
                [{Columns.StatisticValueColumn}] INT NOT NULL,
                UNIQUE([{Columns.LocationIdColumn}],[{Columns.StatisticTypeIdColumn}]),
                FOREIGN KEY ([{Columns.LocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}]),
                FOREIGN KEY ([{Columns.StatisticTypeIdColumn}]) REFERENCES [{Tables.StatisticTypes}]([{Columns.StatisticTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Routes}]
            (
                [{Columns.RouteIdColumn}] INTEGER PRIMARY KEY AUTOINCREMENT,
                [{Columns.LocationIdColumn}] INT NOT NULL,
                [{Columns.DirectionIdColumn}] INT NOT NULL,
                [{Columns.RouteTypeIdColumn}] INT NOT NULL,
                [{Columns.ToLocationIdColumn}] INT NOT NULL,
                UNIQUE([{Columns.LocationIdColumn}],[{Columns.DirectionIdColumn}]),
                FOREIGN KEY([{Columns.LocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}]),
                FOREIGN KEY([{Columns.ToLocationIdColumn}]) REFERENCES [{Tables.Locations}]([{Columns.LocationIdColumn}]),
                FOREIGN KEY([{Columns.DirectionIdColumn}]) REFERENCES [{Tables.Directions}]([{Columns.DirectionIdColumn}]),
                FOREIGN KEY([{Columns.RouteTypeIdColumn}]) REFERENCES [{Tables.RouteTypes}]([{Columns.RouteTypeIdColumn}])
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.Lores}]
            (
                [{Columns.LoreIdColumn}] INTEGER PRIMARY KEY,
                [{Columns.ItemNameColumn}] TEXT NOT NULL,
                [{Columns.LoreTextColumn}] TEXT NOT NULL
            )")
        ScaffoldTable(connection, $"CREATE TABLE [{Tables.ItemLores}]
            (
                [{Columns.LoreIdColumn}] INT NOT NULL UNIQUE,
                [{Columns.ItemIdColumn}] INT NOT NULL UNIQUE,
                FOREIGN KEY ([{Columns.LoreIdColumn}]) REFERENCES [{Tables.Lores}]([{Columns.LoreIdColumn}]),
                FOREIGN KEY ([{Columns.ItemIdColumn}]) REFERENCES [{Tables.Items}]([{Columns.ItemIdColumn}])
            )")
    End Sub
End Module
