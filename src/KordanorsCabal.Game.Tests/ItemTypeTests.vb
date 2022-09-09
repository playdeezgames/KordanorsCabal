Public Class ItemTypeTests
    Private Shared Sub WithExisting(itemTypeId As Long, stuffToDo As Action(Of Mock(Of IWorldData), ItemType))
        Dim worldData As New Mock(Of IWorldData)
        worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
        worldData.SetupGet(Function(x) x.ItemTypeSpawnLocationType).Returns((New Mock(Of IItemTypeSpawnLocationTypeData)).Object)
        Dim subject = ItemType.FromId(worldData.Object, itemTypeId)
        stuffToDo(worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldConstructFromWorldDataAndAnItemTypeId()
        Dim itemTypeId = 1L
        WithExisting(
            itemTypeId,
            Sub(worldData, subject)
                subject.Id.ShouldBe(itemTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeName()
        Dim itemTypeId = 1L
        WithExisting(
            itemTypeId,
            Sub(worldData, subject)
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeIsConsumedFlag()
        Dim itemTypeId = 1L
        WithExisting(
            itemTypeId,
            Sub(worldData, subject)
                subject.IsConsumed.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemType.ReadIsConsumed(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeSpawnLocationTypes()
        Dim itemTypeId = 1L
        Dim dungeonLevelId = 2L
        WithExisting(
            itemTypeId,
            Sub(worldData, subject)
                Dim dungeonLevel = New DungeonLevel(worldData.Object, dungeonLevelId)
                subject.SpawnLocationTypes(dungeonLevel).ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemTypeSpawnLocationType.ReadAll(itemTypeId, dungeonLevelId))
            End Sub)
    End Sub
End Class
