Imports Microsoft.Data.Sqlite

Public Class WorldData
    Private ReadOnly Store As Store
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
    Public ReadOnly CharacterTypeEnemy As CharacterTypeEnemyData
    Public ReadOnly CharacterTypeInitialStatistic As CharacterTypeInitialStatisticData
    Public ReadOnly CharacterTypeLoot As CharacterTypeLootData
    Public ReadOnly CharacterTypePartingShot As CharacterTypePartingShotData
    Public ReadOnly CharacterTypeSpawnLocation As CharacterTypeSpawnLocationData
    Public ReadOnly Direction As DirectionData
    Public ReadOnly DungeonLevel As DungeonLevelData
    Public ReadOnly EquipSlot As EquipSlotData
    Public ReadOnly Feature As FeatureData
    Public ReadOnly FeatureType As FeatureTypeData
    Public ReadOnly Inventory As InventoryData
    Public ReadOnly InventoryItem As InventoryItemData
    Public ReadOnly Item As ItemData
    Public ReadOnly ItemStatistic As ItemStatisticData
    Public ReadOnly ItemType As ItemTypeData
    Public ReadOnly ItemTypeSpawnCount As ItemTypeSpawnCountData
    Public ReadOnly ItemTypeSpawnLocationType As ItemTypeSpawnLocationTypeData
    Public ReadOnly ItemTypeStatisticType As ItemTypeStatisticTypeData
    Public ReadOnly Location As LocationData
    Public ReadOnly LocationDungeonLevel As LocationDungeonLevelData
    Public ReadOnly LocationStatistic As LocationStatisticData
    Public ReadOnly LocationType As LocationTypeData
    Public ReadOnly Player As PlayerData
    Public ReadOnly Route As RouteData

    Public Sub New(store As Store)
        Me.Store = store
        Character = New CharacterData(store, Me)
        CharacterEquipSlot = New CharacterEquipSlotData(store, Me)
        CharacterLocation = New CharacterLocationData(store, Me)
        CharacterQuestCompletion = New CharacterQuestCompletionData(store, Me)
        CharacterQuest = New CharacterQuestData(store, Me)
        CharacterSpell = New CharacterSpellData(store, Me)
        CharacterStatistic = New CharacterStatisticData(store, Me)
        CharacterStatisticType = New CharacterStatisticTypeData(store, Me)
        CharacterType = New CharacterTypeData(store, Me)
        CharacterTypeAttackType = New CharacterTypeAttackTypeData(store, Me)
        CharacterTypeBribe = New CharacterTypeBribeData(store, Me)
        CharacterTypeEnemy = New CharacterTypeEnemyData(store, Me)
        CharacterTypeInitialStatistic = New CharacterTypeInitialStatisticData(store, Me)
        CharacterTypeLoot = New CharacterTypeLootData(store, Me)
        CharacterTypePartingShot = New CharacterTypePartingShotData(store, Me)
        CharacterTypeSpawnCount = New CharacterTypeSpawnCountData(store, Me)
        CharacterTypeSpawnLocation = New CharacterTypeSpawnLocationData(store, Me)
        Direction = New DirectionData(store, Me)
        DungeonLevel = New DungeonLevelData(store, Me)
        EquipSlot = New EquipSlotData(store, Me)
        Feature = New FeatureData(store, Me)
        FeatureType = New FeatureTypeData(store, Me)
        Inventory = New InventoryData(store, Me)
        InventoryItem = New InventoryItemData(store, Me)
        Item = New ItemData(store, Me)
        ItemStatistic = New ItemStatisticData(store, Me)
        ItemType = New ItemTypeData(store, Me)
        ItemTypeSpawnCount = New ItemTypeSpawnCountData(store, Me)
        ItemTypeSpawnLocationType = New ItemTypeSpawnLocationTypeData(store, Me)
        ItemTypeStatisticType = New ItemTypeStatisticTypeData(store, Me)
        Location = New LocationData(store, Me)
        LocationType = New LocationTypeData(store, Me)
        LocationDungeonLevel = New LocationDungeonLevelData(store, Me)
        LocationStatistic = New LocationStatisticData(store, Me)
        Player = New PlayerData(store, Me)
        Route = New RouteData(store, Me)
    End Sub

    Public Sub Save(filename As String)
        Store.Save(filename)
    End Sub

    Public Sub Load(filename As String)
        Store.Load(filename)
    End Sub

    Public Sub Reset()
        Store.Reset()
    End Sub

    Public Function Renew() As Microsoft.Data.Sqlite.SqliteConnection
        Return Store.Renew()
    End Function

    Public ReadOnly CharacterTypeSpawnCount As CharacterTypeSpawnCountData

    Public Sub Restore(oldConnection As SqliteConnection)
        Store.Restore(oldConnection)
    End Sub
End Class
