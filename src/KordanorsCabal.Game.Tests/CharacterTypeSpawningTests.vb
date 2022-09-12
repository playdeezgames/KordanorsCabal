Public Class CharacterTypeSpawningTests
    Private Shared Sub WithAnyCharacterTypeSpawning(stuffToDo As Action(Of Long, Mock(Of IWorldData), ICharacterTypeSpawning))
        Dim characterTypeId = 1
        Dim worldData As New Mock(Of IWorldData)
        Dim subject As ICharacterTypeSpawning = CharacterTypeSpawning.FromId(worldData.Object, characterTypeId)
        stuffToDo(characterTypeId, worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldQueryForTheAbilityToSpawnAGivenCharacterTypeAtAGivenLocationTypeAndAGivenDungeonLevel()
        WithAnyCharacterTypeSpawning(
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

End Class
