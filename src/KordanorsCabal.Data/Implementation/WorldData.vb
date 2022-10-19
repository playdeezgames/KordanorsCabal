Public Class WorldData
    Implements IWorldData
    Private ReadOnly Store As IStore
    Public ReadOnly Property Character As ICharacterData Implements IWorldData.Character
    Public ReadOnly Property CharacterLocation As ICharacterLocationData Implements IWorldData.CharacterLocation
    Public ReadOnly Property CharacterQuestCompletion As ICharacterQuestCompletionData Implements IWorldData.CharacterQuestCompletion
    Public ReadOnly Property CharacterQuest As ICharacterQuestData Implements IWorldData.CharacterQuest
    Public ReadOnly Property CharacterSpell As ICharacterSpellData Implements IWorldData.CharacterSpell
    Public ReadOnly Property CharacterStatistic As ICharacterStatisticData Implements IWorldData.CharacterStatistic
    Public ReadOnly Property CharacterStatisticType As ICharacterStatisticTypeData Implements IWorldData.CharacterStatisticType
    Public ReadOnly Property CharacterType As ICharacterTypeData Implements IWorldData.CharacterType
    Public ReadOnly Property CharacterTypeAttackType As ICharacterTypeAttackTypeData Implements IWorldData.CharacterTypeAttackType
    Public ReadOnly Property CharacterTypeBribe As ICharacterTypeBribeData Implements IWorldData.CharacterTypeBribe
    Public ReadOnly Property CharacterTypeEnemy As ICharacterTypeEnemyData Implements IWorldData.CharacterTypeEnemy
    Public ReadOnly Property CharacterTypeInitialStatistic As ICharacterTypeInitialStatisticData Implements IWorldData.CharacterTypeInitialStatistic
    Public ReadOnly Property CharacterTypeLoot As ICharacterTypeLootData Implements IWorldData.CharacterTypeLoot
    Public ReadOnly Property CharacterTypePartingShot As ICharacterTypePartingShotData Implements IWorldData.CharacterTypePartingShot
    Public ReadOnly Property CharacterTypeSpawnCount As ICharacterTypeSpawnCountData Implements IWorldData.CharacterTypeSpawnCount
    Public ReadOnly Property CharacterTypeSpawnLocation As ICharacterTypeSpawnLocationData Implements IWorldData.CharacterTypeSpawnLocation
    Public ReadOnly Property Direction As IDirectionData Implements IWorldData.Direction
    Public ReadOnly Property DungeonLevel As IDungeonLevelData Implements IWorldData.DungeonLevel
    Public ReadOnly Property EquipSlot As IEquipSlotData Implements IWorldData.EquipSlot
    Public ReadOnly Property Feature As IFeatureData Implements IWorldData.Feature
    Public ReadOnly Property FeatureType As IFeatureTypeData Implements IWorldData.FeatureType
    Public ReadOnly Property Inventory As IInventoryData Implements IWorldData.Inventory
    Public ReadOnly Property InventoryItem As IInventoryItemData Implements IWorldData.InventoryItem
    Public ReadOnly Property Item As IItemData Implements IWorldData.Item
    Public ReadOnly Property ItemStatistic As IItemStatisticData Implements IWorldData.ItemStatistic
    Public ReadOnly Property ItemType As IItemTypeData Implements IWorldData.ItemType
    Public ReadOnly Property ItemTypeSpawnCount As IItemTypeSpawnCountData Implements IWorldData.ItemTypeSpawnCount
    Public ReadOnly Property ItemTypeSpawnLocationType As IItemTypeSpawnLocationTypeData Implements IWorldData.ItemTypeSpawnLocationType
    Public ReadOnly Property ItemTypeStatistic As IItemTypeStatisticData Implements IWorldData.ItemTypeStatistic
    Public ReadOnly Property ItemTypeStatisticType As IItemTypeStatisticTypeData Implements IWorldData.ItemTypeStatisticType
    Public ReadOnly Property Location As ILocationData Implements IWorldData.Location
    Public ReadOnly Property LocationDungeonLevel As ILocationDungeonLevelData Implements IWorldData.LocationDungeonLevel
    Public ReadOnly Property LocationStatistic As ILocationStatisticData Implements IWorldData.LocationStatistic
    Public ReadOnly Property LocationType As ILocationTypeData Implements IWorldData.LocationType
    Public ReadOnly Property Player As IPlayerData Implements IWorldData.Player
    Public ReadOnly Property Route As IRouteData Implements IWorldData.Route
    Public ReadOnly Property SpellType As ISpellTypeData Implements IWorldData.SpellType
    Public ReadOnly Property SpellTypeRequiredPower As ISpellTypeRequiredPowerData Implements IWorldData.SpellTypeRequiredPower
    Public ReadOnly Property Events As IEventData Implements IWorldData.Events
    Public ReadOnly Property ItemTypeCharacterStatisticBuff As IItemTypeCharacterStatisticBuffData Implements IWorldData.ItemTypeCharacterStatisticBuff
    Public ReadOnly Property ItemTypeEquipSlot As IItemTypeEquipSlotData Implements IWorldData.ItemTypeEquipSlot
    Public ReadOnly Property ItemTypeShopType As IItemTypeShopTypeData Implements IWorldData.ItemTypeShopType
    Public ReadOnly Property ItemTypeEvent As IItemTypeEventData Implements IWorldData.ItemTypeEvent
    Public ReadOnly Property ShoppeType As IShoppeTypeData Implements IWorldData.ShoppeType
    Public ReadOnly Property QuestType As IQuestTypeData Implements IWorldData.QuestType
    Public ReadOnly Property RouteType As IRouteTypeData Implements IWorldData.RouteType
    Public ReadOnly Property RouteTypeLock As IRouteTypeLockData Implements IWorldData.RouteTypeLock
    Public ReadOnly Property ItemStatisticType As IItemStatisticTypeData Implements IWorldData.ItemStatisticType

    Public Sub New(store As IStore, events As IEventData)
        Me.Store = store
        Me.Events = events
        Character = New CharacterData(store, Me)
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
        ItemStatisticType = New ItemStatisticTypeData(store, Me)
        ItemType = New ItemTypeData(store, Me)
        ItemTypeCharacterStatisticBuff = New ItemTypeCharacterStatisticBuffData(store, Me)
        ItemTypeEquipSlot = New ItemTypeEquipSlotData(store, Me)
        ItemTypeEvent = New ItemTypeEventData(store, Me)
        ItemTypeShopType = New ItemTypeShopTypeData(store, Me)
        ItemTypeSpawnCount = New ItemTypeSpawnCountData(store, Me)
        ItemTypeSpawnLocationType = New ItemTypeSpawnLocationTypeData(store, Me)
        ItemTypeStatistic = New ItemTypeStatisticData(store, Me)
        ItemTypeStatisticType = New ItemTypeStatisticTypeData(store, Me)
        Location = New LocationData(store, Me)
        LocationType = New LocationTypeData(store, Me)
        LocationDungeonLevel = New LocationDungeonLevelData(store, Me)
        LocationStatistic = New LocationStatisticData(store, Me)
        Player = New PlayerData(store, Me)
        QuestType = New QuestTypeData(store, Me)
        Route = New RouteData(store, Me)
        RouteType = New RouteTypeData(store, Me)
        RouteTypeLock = New RouteTypeLockData(store, Me)
        ShoppeType = New ShoppeTypeData(store, Me)
        SpellType = New SpellTypeData(store, Me)
        SpellTypeRequiredPower = New SpellTypeRequiredPowerData(store, Me)
    End Sub

    Public Sub Save(filename As String) Implements IWorldData.Save
        Store.Meta.Save(filename)
    End Sub

    Public Sub Load(filename As String) Implements IWorldData.Load
        Store.Meta.Load(filename)
    End Sub

    Public Sub Reset() Implements IWorldData.Reset
        Store.Meta.Reset()
    End Sub

    Public Function Renew() As IBacker Implements IWorldData.Renew
        Return Store.Meta.Renew()
    End Function


    Public Sub Restore(oldBacker As IBacker) Implements IWorldData.Restore
        Store.Meta.Restore(oldBacker)
    End Sub
End Class
