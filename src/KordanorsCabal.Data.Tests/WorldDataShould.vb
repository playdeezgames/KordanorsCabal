Public Class WorldDataShould
    <Fact>
    Sub ShouldConstructAllOfTheDataSubobjects()
        Dim store As New Mock(Of IStore)
        Dim checker As New Mock(Of IEventData)
        Dim subject As IWorldData = New WorldData(store.Object, checker.Object)
        subject.Character.ShouldNotBeNull
        subject.CharacterEquipSlot.ShouldNotBeNull
        subject.CharacterLocation.ShouldNotBeNull
        subject.CharacterQuestCompletion.ShouldNotBeNull
        subject.CharacterQuest.ShouldNotBeNull
        subject.CharacterSpell.ShouldNotBeNull
        subject.CharacterStatistic.ShouldNotBeNull
        subject.CharacterStatisticType.ShouldNotBeNull
        subject.CharacterType.ShouldNotBeNull
        subject.CharacterTypeAttackType.ShouldNotBeNull
        subject.CharacterTypeBribe.ShouldNotBeNull
        subject.CharacterTypeEnemy.ShouldNotBeNull
        subject.CharacterTypeInitialStatistic.ShouldNotBeNull
        subject.CharacterTypeLoot.ShouldNotBeNull
        subject.CharacterTypePartingShot.ShouldNotBeNull
        subject.CharacterTypeSpawnCount.ShouldNotBeNull
        subject.CharacterTypeSpawnLocation.ShouldNotBeNull
        subject.Direction.ShouldNotBeNull
        subject.DungeonLevel.ShouldNotBeNull
        subject.EquipSlot.ShouldNotBeNull
        subject.Feature.ShouldNotBeNull
        subject.FeatureType.ShouldNotBeNull
        subject.Inventory.ShouldNotBeNull
        subject.InventoryItem.ShouldNotBeNull
        subject.Item.ShouldNotBeNull
        subject.ItemStatistic.ShouldNotBeNull
        subject.ItemStatisticType.ShouldNotBeNull
        subject.ItemType.ShouldNotBeNull
        subject.ItemTypeCharacterStatisticBuff.ShouldNotBeNull
        subject.ItemTypeEquipSlot.ShouldNotBeNull
        subject.ItemTypeEvent.ShouldNotBeNull
        subject.ItemTypeShopType.ShouldNotBeNull
        subject.ItemTypeSpawnCount.ShouldNotBeNull
        subject.ItemTypeSpawnLocationType.ShouldNotBeNull
        subject.ItemTypeStatistic.ShouldNotBeNull
        subject.ItemTypeStatisticType.ShouldNotBeNull
        subject.Location.ShouldNotBeNull
        subject.LocationDungeonLevel.ShouldNotBeNull
        subject.LocationStatistic.ShouldNotBeNull
        subject.LocationType.ShouldNotBeNull
        subject.Player.ShouldNotBeNull
        subject.QuestType.ShouldNotBeNull
        subject.Route.ShouldNotBeNull
        subject.RouteType.ShouldNotBeNull
        subject.RouteTypeLock.ShouldNotBeNull
        subject.ShoppeType.ShouldNotBeNull
        subject.SpellType.ShouldNotBeNull
        subject.SpellTypeRequiredPower.ShouldNotBeNull
        subject.Events.ShouldNotBeNull
        store.VerifyNoOtherCalls()
        checker.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldSaveToTheStore()
        Dim store As New Mock(Of IStore)
        Dim checker As New Mock(Of IEventData)
        Dim subject As IWorldData = New WorldData(store.Object, checker.Object)
        Const filename = "filename"
        subject.Save(filename)
        store.Verify(Sub(x) x.Save(filename), Times.Once)
        store.VerifyNoOtherCalls()
        checker.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldLoadFromTheStore()
        Dim store As New Mock(Of IStore)
        Dim checker As New Mock(Of IEventData)
        Dim subject As IWorldData = New WorldData(store.Object, checker.Object)
        Const filename = "filename"
        subject.Load(filename)
        store.Verify(Sub(x) x.Load(filename), Times.Once)
        store.VerifyNoOtherCalls()
        checker.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldResetTheStore()
        Dim store As New Mock(Of IStore)
        Dim checker As New Mock(Of IEventData)
        Dim subject As IWorldData = New WorldData(store.Object, checker.Object)
        subject.Reset()
        store.Verify(Sub(x) x.Reset(), Times.Once)
        store.VerifyNoOtherCalls()
        checker.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldRenewTheStore()
        Dim store As New Mock(Of IStore)
        Dim checker As New Mock(Of IEventData)
        Dim subject As IWorldData = New WorldData(store.Object, checker.Object)
        subject.Renew().ShouldBeNull
        store.Verify(Function(x) x.Renew(), Times.Once)
        store.VerifyNoOtherCalls()
        checker.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldRestoreTheStore()
        Dim store As New Mock(Of IStore)
        Dim checker As New Mock(Of IEventData)
        Dim subject As IWorldData = New WorldData(store.Object, checker.Object)
        subject.Restore(Nothing)
        store.Verify(Sub(x) x.Restore(Nothing), Times.Once)
        store.VerifyNoOtherCalls()
        checker.VerifyNoOtherCalls()
    End Sub
End Class


