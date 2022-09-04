﻿Public Interface IWorldData
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
    ReadOnly Property ItemTypeSpawnCount As ItemTypeSpawnCountData
    ReadOnly Property ItemTypeSpawnLocationType As ItemTypeSpawnLocationTypeData
    ReadOnly Property ItemTypeStatistic As ItemTypeStatisticData
    ReadOnly Property ItemTypeStatisticType As ItemTypeStatisticTypeData
    ReadOnly Property Location As LocationData
    ReadOnly Property LocationDungeonLevel As LocationDungeonLevelData
    ReadOnly Property LocationStatistic As LocationStatisticData
    ReadOnly Property LocationType As LocationTypeData
    ReadOnly Property Player As IPlayerData
    ReadOnly Property Route As RouteData
End Interface
