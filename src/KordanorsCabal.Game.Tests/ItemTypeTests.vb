Public Class ItemTypeTests
    Inherits BaseThingieTests(Of IItemType)
    Sub New()
        MyBase.New(AddressOf ItemType.FromId)
    End Sub
    <Fact>
    Sub ShouldConstructFromWorldDataAndAnItemTypeId()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                subject.Id.ShouldBe(itemTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeName()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeIsConsumedFlag()
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
                subject.IsConsumed.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemType.ReadIsConsumed(itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryTheWorldDataForItemTypeSpawnLocationTypes()
        Dim dungeonLevelId = 2L
        WithAnySubject(
            Sub(itemTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.ItemTypeSpawnLocationType).Returns((New Mock(Of IItemTypeSpawnLocationTypeData)).Object)
                Dim dungeonLevel = New DungeonLevel(worldData.Object, dungeonLevelId)
                subject.SpawnLocationTypes(dungeonLevel).ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemTypeSpawnLocationType.ReadAll(itemTypeId, dungeonLevelId))
            End Sub)
    End Sub
End Class
