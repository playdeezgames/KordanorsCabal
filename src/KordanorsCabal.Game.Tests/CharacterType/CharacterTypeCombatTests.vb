Public Class CharacterTypeCombatTests
    Inherits BaseThingieTests(Of ICharacterTypeCombat)

    Public Sub New()
        MyBase.New(AddressOf CharacterTypeCombat.FromId)
    End Sub

    <Fact>
    Sub character_types_contain_whether_can_be_bribed_with_an_item_type_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)
                Dim itemTypeId = 14L
                Dim actual = subject.CanBeBribedWith(ItemType.FromId(worldData.Object, itemTypeId))
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub character_types_drop_on_a_location_loot_fetched_from_the_data_store()
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
    Sub character_types_generate_attack_types_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeAttackType).Returns((New Mock(Of ICharacterTypeAttackTypeData)).Object)
                Dim actual = subject.GenerateAttackType()
                actual.ShouldBe(AttackType.None)
                worldData.Verify(Function(x) x.CharacterTypeAttackType.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub character_types_contain_whether_another_character_type_is_an_enemy_fetched_from_the_data_store()
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
    Sub character_types_generate_a_parting_shot_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypePartingShot).Returns((New Mock(Of ICharacterTypePartingShotData)).Object)
                Dim actual = subject.PartingShot
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterTypePartingShot.Read(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub character_types_generate_a_money_drop_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.RollMoneyDrop()
                actual.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterType.ReadMoneyDropDice(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub character_types_contain_xp_values_fetched_from_the_data_store()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.XPValue
                actual.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterType.ReadXPValue(characterTypeId))
            End Sub)
    End Sub
End Class
