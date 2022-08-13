Public Class WorldData
    Public ReadOnly Character As CharacterData
    Public ReadOnly CharacterEquipSlot As CharacterEquipSlotData
    Public ReadOnly CharacterLocation As CharacterLocationData
    Public ReadOnly CharacterQuestCompletion As CharacterQuestCompletionData
    Public ReadOnly CharacterQuest As CharacterQuestData
    Public ReadOnly CharacterSpell As CharacterSpellData
    Public ReadOnly CharacterStatistic As CharacterStatisticData
    Public ReadOnly CharacterStatisticType As CharacterStatisticTypeData
    Public ReadOnly CharacterType As CharacterTypeData
    Public ReadOnly CharacterTypeAttackType As CharacterTypeAttackTypeData
    Public ReadOnly CharacterTypeBribe As CharacterTypeBribeData
    Public ReadOnly CharacterTypeInitialStatistic As CharacterTypeInitialStatisticData
    Public ReadOnly CharacterTypeLoot As CharacterTypeLootData
    Public ReadOnly CharacterTypePartingShot As CharacterTypePartingShotData
    Public ReadOnly CharacterTypeSpawnCount As CharacterTypeSpawnCountData
    Public ReadOnly Direction As DirectionData
    Public ReadOnly DungeonLevel As DungeonLevelData
    Public ReadOnly EquipSlot As EquipSlotData
    Public ReadOnly Feature As FeatureData
    Public ReadOnly Inventory As InventoryData
    Public ReadOnly InventoryItem As InventoryItemData
    Public ReadOnly Item As ItemData
    Public ReadOnly ItemStatistic As ItemStatisticData
    Public ReadOnly Location As LocationData
    Public ReadOnly LocationDungeonLevel As LocationDungeonLevelData
    Public ReadOnly LocationStatistic As LocationStatisticData
    Public ReadOnly LocationType As LocationTypeData
    Public ReadOnly Player As PlayerData
    Public ReadOnly Route As RouteData

    Public Sub New(store As Store)
        Character = New CharacterData(store)
        CharacterEquipSlot = New CharacterEquipSlotData(store)
        CharacterLocation = New CharacterLocationData(store)
        CharacterQuestCompletion = New CharacterQuestCompletionData(store)
        CharacterQuest = New CharacterQuestData(store)
        CharacterSpell = New CharacterSpellData(store)
        CharacterStatistic = New CharacterStatisticData(store)
        CharacterStatisticType = New CharacterStatisticTypeData(store)
        CharacterType = New CharacterTypeData(store)
        CharacterTypeAttackType = New CharacterTypeAttackTypeData(store)
        CharacterTypeBribe = New CharacterTypeBribeData(store)
        CharacterTypeInitialStatistic = New CharacterTypeInitialStatisticData(store)
        CharacterTypeLoot = New CharacterTypeLootData(store)
        CharacterTypePartingShot = New CharacterTypePartingShotData(store)
        CharacterTypeSpawnCount = New CharacterTypeSpawnCountData(store)
        Direction = New DirectionData(store)
        DungeonLevel = New DungeonLevelData(store)
        EquipSlot = New EquipSlotData(store)
        Feature = New FeatureData(store)
        Inventory = New InventoryData(store)
        InventoryItem = New InventoryItemData(store)
        Item = New ItemData(store)
        ItemStatistic = New ItemStatisticData(store)
        Location = New LocationData(store)
        LocationType = New LocationTypeData(store)
        LocationDungeonLevel = New LocationDungeonLevelData(store)
        LocationStatistic = New LocationStatisticData(store)
        Player = New PlayerData(store)
        Route = New RouteData(store)
    End Sub
End Class
