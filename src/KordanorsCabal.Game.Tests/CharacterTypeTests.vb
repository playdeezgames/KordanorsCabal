Public Class CharacterTypeTests
    Private Shared Sub WithAnyCharacterType(stuffToDo As Action(Of Long, Mock(Of IWorldData), ICharacterType))
        Dim characterTypeId = 1
        Dim worldData As New Mock(Of IWorldData)
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
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.IsUndead
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForAttackTypeGenerationWeightsAndThenGenerate()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeAttackType).Returns((New Mock(Of ICharacterTypeAttackTypeData)).Object)
                Dim actual = subject.GenerateAttackType()
                actual.ShouldBe(AttackType.None)
                worldData.Verify(Function(x) x.CharacterTypeAttackType.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForWhetherAGivenCharacterIsAnEnemyOfAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeEnemy).Returns((New Mock(Of ICharacterTypeEnemyData)).Object)
                Dim otherCharacterTypeId = 2L
                Dim otherCharacterType As New Mock(Of ICharacterType)
                otherCharacterType.SetupGet(Function(x) x.Id).Returns(otherCharacterTypeId)
                subject.IsEnemy(otherCharacterType.Object).ShouldBeFalse
                otherCharacterType.VerifyGet(Function(x) x.Id)
                otherCharacterType.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterTypeEnemy.Read(characterTypeId, otherCharacterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForNameOfAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.Name
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterType.ReadName(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForPartingShotOfAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypePartingShot).Returns((New Mock(Of ICharacterTypePartingShotData)).Object)
                Dim actual = subject.PartingShot
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterTypePartingShot.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForMoneyDropOfAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.RollMoneyDrop()
                actual.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterType.ReadMoneyDropDice(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForXPValueOfAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.XPValue
                actual.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterType.ReadXPValue(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveSpawningSubobjectFromAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                subject.Spawning.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveCombatSubobjectFromAGivenCharacterType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                subject.Combat.ShouldNotBeNull
            End Sub)
    End Sub
End Class


