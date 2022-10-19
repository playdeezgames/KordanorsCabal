Public Class ItemTypeSpawnShould
    Inherits ThingieShould(Of IItemTypeSpawn)
    Public Sub New()
        MyBase.New(Function(w, i) ItemTypeSpawn.FromId(w, i))
    End Sub
    <Fact>
    Sub item_types_rolls_spawn_counts()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                Const dungeonLevelId = 2L
                worldData.Setup(Function(x) x.ItemTypeSpawnCount.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.RollSpawnCount(DungeonLevel.FromId(worldData.Object, dungeonLevelId)).ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeSpawnCount.Read(itemTypeId, dungeonLevelId))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_spawn_locations_by_dungeon_level_fetched_from_the_data_store()
        Dim dungeonLevelId = 2L
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.SetupGet(Function(x) x.ItemTypeSpawnLocationType).Returns((New Mock(Of IItemTypeSpawnLocationTypeData)).Object)
                Dim dungeonLevel = New DungeonLevel(worldData.Object, dungeonLevelId)
                subject.SpawnLocationTypes(dungeonLevel).ShouldBeEmpty
                worldData.Verify(Function(x) x.ItemTypeSpawnLocationType.ReadAll(itemTypeId, dungeonLevelId))
            End Sub)
    End Sub
End Class
