Public Interface IWorldData
    ReadOnly Property Character As ICharacterData
    ReadOnly Property CharacterEquipSlot As ICharacterEquipSlotData
    ReadOnly Property CharacterLocation As ICharacterLocationData
    ReadOnly Property CharacterQuestCompletion As ICharacterQuestCompletionData
    ReadOnly Property CharacterQuest As ICharacterQuestData
    ReadOnly Property CharacterSpell As ICharacterSpellData
    ReadOnly Property CharacterStatistic As ICharacterStatisticData
    ReadOnly Property CharacterStatisticType As ICharacterStatisticTypeData
    ReadOnly Property CharacterType As ICharacterTypeData
    ReadOnly Property CharacterTypeAttackType As ICharacterTypeAttackTypeData
    ReadOnly Property CharacterTypeBribe As ICharacterTypeBribeData
    ReadOnly Property CharacterTypeEnemy As ICharacterTypeEnemyData
    ReadOnly Property CharacterTypeInitialStatistic As ICharacterTypeInitialStatisticData
    ReadOnly Property CharacterTypeLoot As ICharacterTypeLootData
    ReadOnly Property CharacterTypePartingShot As ICharacterTypePartingShotData
    ReadOnly Property CharacterTypeSpawnCount As ICharacterTypeSpawnCountData
    ReadOnly Property CharacterTypeSpawnLocation As ICharacterTypeSpawnLocationData
    ReadOnly Property Direction As IDirectionData
    ReadOnly Property DungeonLevel As IDungeonLevelData
    ReadOnly Property EquipSlot As IEquipSlotData
    ReadOnly Property Feature As IFeatureData
    ReadOnly Property FeatureType As IFeatureTypeData
    ReadOnly Property Inventory As IInventoryData
    ReadOnly Property InventoryItem As IInventoryItemData
    ReadOnly Property Item As IItemData
    ReadOnly Property ItemStatistic As IItemStatisticData
    ReadOnly Property ItemType As IItemTypeData
    ReadOnly Property ItemTypeCharacterStatisticBuff As IItemTypeCharacterStatisticBuffData
    ReadOnly Property ItemTypeEvent As IItemTypeEventData
    ReadOnly Property ItemTypeShopType As IItemTypeShopTypeData
    ReadOnly Property ItemTypeSpawnCount As IItemTypeSpawnCountData
    ReadOnly Property ItemTypeSpawnLocationType As IItemTypeSpawnLocationTypeData
    ReadOnly Property ItemTypeStatistic As IItemTypeStatisticData
    ReadOnly Property ItemTypeStatisticType As IItemTypeStatisticTypeData
    ReadOnly Property Location As ILocationData
    ReadOnly Property LocationDungeonLevel As ILocationDungeonLevelData
    ReadOnly Property LocationStatistic As ILocationStatisticData
    ReadOnly Property LocationType As ILocationTypeData
    ReadOnly Property Player As IPlayerData
    ReadOnly Property QuestType As IQuestTypeData
    ReadOnly Property Route As IRouteData
    ReadOnly Property SpellType As ISpellTypeData
    ReadOnly Property SpellTypeRequiredPower As ISpellTypeRequiredPowerData
    ReadOnly Property Events As IEventData
    ReadOnly Property ItemTypeEquipSlot As IItemTypeEquipSlotData
    ReadOnly Property ShoppeType As IShoppeTypeData
    Sub Save(filename As String)
    Sub Load(filename As String)
    Sub Reset()
    Function Renew() As Microsoft.Data.Sqlite.SqliteConnection
    Sub Restore(p As Microsoft.Data.Sqlite.SqliteConnection)
    ReadOnly Property RouteType As IRouteTypeData
End Interface
