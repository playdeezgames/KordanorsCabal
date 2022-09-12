Public Class CharacterTypeCombatTests
    Private Shared Sub WithAnySubject(stuffToDo As Action(Of Long, Mock(Of IWorldData), ICharacterTypeCombat))
        Dim characterTypeId = 1
        Dim worldData As New Mock(Of IWorldData)
        Dim subject As ICharacterTypeCombat = CharacterTypeCombat.FromId(worldData.Object, characterTypeId)
        stuffToDo(characterTypeId, worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldQueryForTheAbilityToBribeAGivenCharacterTypeWitAGivenItemType()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)
                Dim itemTypeId = OldItemType.AirShard
                Dim actual = subject.CanBeBribedWith(itemTypeId)
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDropLootForAGivenCharacterTypeOntoAGivenLocation()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeLoot).Returns((New Mock(Of ICharacterTypeLootData)).Object)
                Dim location As New Mock(Of ILocation)
                subject.DropLoot(location.Object)
                location.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterTypeLoot.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForAttackTypeGenerationWeightsAndThenGenerate()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeAttackType).Returns((New Mock(Of ICharacterTypeAttackTypeData)).Object)
                Dim actual = subject.GenerateAttackType()
                actual.ShouldBe(AttackType.None)
                worldData.Verify(Function(x) x.CharacterTypeAttackType.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForWhetherAGivenCharacterIsAnEnemyOfAGivenCharacterType()
        WithAnySubject(
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
    Sub ShouldQueryForPartingShotOfAGivenCharacterType()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypePartingShot).Returns((New Mock(Of ICharacterTypePartingShotData)).Object)
                Dim actual = subject.PartingShot
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterTypePartingShot.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForMoneyDropOfAGivenCharacterType()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.RollMoneyDrop()
                actual.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterType.ReadMoneyDropDice(characterTypeId))
            End Sub)
    End Sub
End Class
