Public Class CharacterPhysicalCombatShould
    Inherits ThingieShould(Of ICharacterPhysicalCombat)
    Sub New()
        MyBase.New(Function(w, i) CharacterPhysicalCombat.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub run()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.Run()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub roll_defend()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollDefend().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 2))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 11))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(2))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(11))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub roll_attack()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollAttack().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 1))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(1))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_parting_shot()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 2L
                worldData.Setup(Function(x) x.Character.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.Setup(Function(x) x.CharacterTypePartingShot.Read(It.IsAny(Of Long)))
                subject.PartingShot.ShouldBeNull
                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterTypePartingShot.Read(characterTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub kill()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                Const characterTypeId = 3L
                worldData.Setup(Function(x) x.Character.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.Setup(Function(x) x.CharacterType.ReadName(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterTypeLoot.Read(It.IsAny(Of Long)))
                Dim actual = subject.Kill(Character.FromId(worldData.Object, characterId))
                actual.Item1.ShouldBe(Sfx.EnemyDeath)
                actual.Item2.ShouldNotBeEmpty
                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Sub(x) x.Character.Clear(id))
                worldData.Verify(Function(x) x.CharacterType.ReadName(characterTypeId))
                worldData.Verify(Function(x) x.CharacterType.ReadMoneyDropDice(characterTypeId))
                worldData.Verify(Function(x) x.CharacterType.ReadXPValue(characterTypeId))
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.CharacterTypeLoot.Read(characterTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_is_enemy()
        WithSubject(
            Sub(worldData, id, subject)
                Const otherCharacterId = 2L
                Const characterTypeId = 3L
                Const otherCharacterTypeId = 4L
                Dim otherCharacter = Character.FromId(worldData.Object, otherCharacterId)
                Dim characterData = FreshMock(Of ICharacterData)()
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                characterData.Setup(Function(x) x.ReadCharacterType(otherCharacterId)).Returns(otherCharacterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(x) x.CharacterTypeEnemy).Returns(FreshMock(Of ICharacterTypeEnemyData).Object)

                subject.IsEnemy(otherCharacter).ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.Character.ReadCharacterType(otherCharacterId))
                worldData.Verify(Function(x) x.CharacterTypeEnemy.Read(characterTypeId, otherCharacterTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub fight()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.Fight()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub do_damage()
        WithSubject(
            Sub(worldData, id, subject)
                Const damage = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoDamage(damage)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 12))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(12))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub do_counter_attacks()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationId = 2L
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long))).Returns(locationId)
                subject.DoCounterAttacks()
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Character.ReadForLocation(locationId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub determine_damage()
        WithSubject(
            Sub(worldData, id, subject)
                Const damage = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                subject.DetermineDamage(damage).ShouldBe(0)
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 10))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(10))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_can_fight()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanFight().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
