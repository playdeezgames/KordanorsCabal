Public Class ItemTypeTests
    Private Shared Sub WithExisting(stuffToDo As Action(Of Long, Mock(Of IWorldData), IItemType))
        Dim itemTypeId = 1L
        Dim worldData As New Mock(Of IWorldData)
        worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
        worldData.SetupGet(Function(x) x.ItemTypeSpawnLocationType).Returns((New Mock(Of IItemTypeSpawnLocationTypeData)).Object)
        Dim subject As IItemType = ItemType.FromId(worldData.Object, itemTypeId)
        stuffToDo(itemTypeId, worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldConstructFromWorldDataAndAnItemTypeId()
        WithExisting(
            Sub(itemTypeId, worldData, subject)
                subject.Id.ShouldBe(itemTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeName()
        WithExisting(
            Sub(itemTypeId, worldData, subject)
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeIsConsumedFlag()
        WithExisting(
            Sub(itemTypeId, worldData, subject)
                subject.IsConsumed.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemType.ReadIsConsumed(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeSpawnLocationTypes()
        Dim dungeonLevelId = 2L
        WithExisting(
            Sub(itemTypeId, worldData, subject)
                Dim dungeonLevel = New DungeonLevel(worldData.Object, dungeonLevelId)
                subject.SpawnLocationTypes(dungeonLevel).ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemTypeSpawnLocationType.ReadAll(itemTypeId, dungeonLevelId))
            End Sub)
    End Sub
End Class
