Public Class CharacterTypeSpawningTests
    Inherits BaseThingieTests(Of ICharacterTypeSpawning)
    Public Sub New()
        MyBase.New(AddressOf CharacterTypeSpawning.FromId)
    End Sub
    <Fact>
    Sub character_types_contains_information_about_spawnability_in_location_types_on_dungeon_levels_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeSpawnLocation).Returns((New Mock(Of ICharacterTypeSpawnLocationData)).Object)
                Dim locationType = Game.LocationType.FromId(worldData.Object, 2L)
                Dim dungeonLevel = Game.DungeonLevel.FromId(worldData.Object, 3L)
                Dim actual = subject.CanSpawn(locationType, dungeonLevel)
                actual.ShouldBeFalse
                worldData.Verify(
                    Function(x) x.CharacterTypeSpawnLocation.Read(
                    characterTypeId,
                    dungeonLevel.Id,
                    locationType.Id))
            End Sub)
    End Sub
    <Fact>
    Sub character_types_contain_per_dungeon_level_spawn_counts_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeSpawnCount).Returns((New Mock(Of ICharacterTypeSpawnCountData)).Object)
                Dim dungeonLevel As New Mock(Of IDungeonLevel)
                Dim dungeonLevelId = 2L
                dungeonLevel.SetupGet(Function(x) x.Id).Returns(dungeonLevelId)
                Dim actual = subject.SpawnCount(dungeonLevel.Object)
                actual.ShouldBe(0)
                dungeonLevel.VerifyGet(Function(x) x.Id)
                dungeonLevel.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterTypeSpawnCount.ReadSpawnCount(characterTypeId, dungeonLevelId))
            End Sub)
    End Sub
    <Fact>
    Sub character_types_contain_initial_statistics_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeInitialStatistic).Returns((New Mock(Of ICharacterTypeInitialStatisticData)).Object)
                Dim actual = subject.InitialStatistics()
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterTypeInitialStatistic.ReadAllForCharacterType(characterTypeId))
            End Sub)
    End Sub
End Class
