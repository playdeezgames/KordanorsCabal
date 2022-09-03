Namespace KordanorsCabal.Data.Tests
    Public Class WorldDataTests
        <Fact>
        Sub ShouldConstructAllOfTheDataSubobjects()
            Dim store As New Mock(Of IStore)
            Dim worldData As New WorldData(store.Object)
            worldData.Character.ShouldNotBeNull
            worldData.CharacterEquipSlot.ShouldNotBeNull
            worldData.CharacterLocation.ShouldNotBeNull
            worldData.CharacterQuestCompletion.ShouldNotBeNull
            worldData.CharacterQuest.ShouldNotBeNull
            worldData.CharacterSpell.ShouldNotBeNull
            worldData.CharacterStatistic.ShouldNotBeNull
            worldData.CharacterStatisticType.ShouldNotBeNull
            worldData.CharacterType.ShouldNotBeNull
            worldData.CharacterTypeAttackType.ShouldNotBeNull
            worldData.CharacterTypeBribe.ShouldNotBeNull
            worldData.CharacterTypeEnemy.ShouldNotBeNull
            worldData.CharacterTypeInitialStatistic.ShouldNotBeNull
            worldData.CharacterTypeLoot.ShouldNotBeNull
            worldData.CharacterTypePartingShot.ShouldNotBeNull
            worldData.CharacterTypeSpawnLocation.ShouldNotBeNull
            worldData.Direction.ShouldNotBeNull
            worldData.DungeonLevel.ShouldNotBeNull
            worldData.EquipSlot.ShouldNotBeNull
            worldData.Feature.ShouldNotBeNull
            worldData.FeatureType.ShouldNotBeNull
            worldData.Inventory.ShouldNotBeNull
            worldData.InventoryItem.ShouldNotBeNull
            worldData.Item.ShouldNotBeNull
            worldData.ItemStatistic.ShouldNotBeNull
            worldData.ItemType.ShouldNotBeNull
            worldData.ItemTypeSpawnCount.ShouldNotBeNull
            worldData.ItemTypeSpawnLocationType.ShouldNotBeNull
            worldData.ItemTypeStatistic.ShouldNotBeNull
            worldData.ItemTypeStatisticType.ShouldNotBeNull
            worldData.Location.ShouldNotBeNull
            worldData.LocationDungeonLevel.ShouldNotBeNull
            worldData.LocationStatistic.ShouldNotBeNull
            worldData.LocationType.ShouldNotBeNull
            worldData.Player.ShouldNotBeNull
            worldData.Route.ShouldNotBeNull
            store.VerifyNoOtherCalls()
        End Sub
    End Class
End Namespace

