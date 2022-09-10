Public Class CharacterTypeTests
    Private Shared Sub WithAnyCharacterType(stuffToDo As Action(Of Long, Mock(Of IWorldData), ICharacterType))
        Dim characterTypeId = 1
        Dim worldData As New Mock(Of IWorldData)
        worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
        worldData.SetupGet(Function(x) x.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)
        worldData.SetupGet(Function(x) x.CharacterTypeSpawnLocation).Returns((New Mock(Of ICharacterTypeSpawnLocationData)).Object)
        worldData.SetupGet(Function(x) x.CharacterTypeAttackType).Returns((New Mock(Of ICharacterTypeAttackTypeData)).Object)
        worldData.SetupGet(Function(x) x.CharacterTypeInitialStatistic).Returns((New Mock(Of ICharacterTypeInitialStatisticData)).Object)
        worldData.SetupGet(Function(x) x.CharacterTypeEnemy).Returns((New Mock(Of ICharacterTypeEnemyData)).Object)
        Dim subject As ICharacterType = CharacterType.FromId(worldData.Object, characterTypeId)
        stuffToDo(characterTypeId, worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldConstructFromWorldDataAndACharacterTypeId()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                subject.Id.ShouldBe(characterTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForUndeadStatusOfAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                Dim actual = subject.IsUndead
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForTheAbilityToBribeAGivenCharacterTypeWitAGivenItemType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                Dim itemTypeId = OldItemType.AirShard
                Dim actual = subject.CanBeBribedWith(itemTypeId)
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForTheAbilityToSpawnAGivenCharacterTypeAtAGivenLocationTypeAndAGivenDungeonLevel()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
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
    Sub ShouldQueryForAttackTypeGenerationWeightsAndThenGenerate()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                Dim actual = subject.GenerateAttackType()
                actual.ShouldBe(AttackType.None)
                worldData.Verify(Function(x) x.CharacterTypeAttackType.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForInitialStatisticsForAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                Dim actual = subject.InitialStatistics()
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterTypeInitialStatistic.ReadAllForCharacterType(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForWhetherAGivenCharacterIsAnEnemyOfAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                Dim otherCharacterTypeId = 2L
                Dim character As New Mock(Of ICharacter)
                Dim otherCharacterType As New Mock(Of ICharacterType)
                otherCharacterType.SetupGet(Function(x) x.Id).Returns(otherCharacterTypeId)
                character.SetupGet(Function(x) x.CharacterType).Returns(otherCharacterType.Object)
                subject.IsEnemy(character.Object).ShouldBeFalse
                character.VerifyGet(Function(x) x.CharacterType.Id)
                character.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterTypeEnemy.Read(characterTypeId, otherCharacterTypeId))
            End Sub)
    End Sub
End Class


