Imports SQLitePCL

Public Class CharacterTests
    Inherits ThingieShould(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
    End Sub
    <Fact>
    Sub ShouldAttemptToPerformQuestAcceptance()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.AcceptQuest(OldQuestType.CellarRats)
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, "CharacterCanAcceptCellarRatsQuest", {1L}))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToChangeValuesAssociatedWithCurrentMP()
        WithSubject(
            Sub(worldData, id, subject)
                Const stress = 2L
                Const statisticTypeId = 13L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.AddStress(stress)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToAddXP()
        WithSubject(
            Sub(worldData, id, subject)
                Const xp = 2L
                Const statisticTypeId = 16L
                Const otherStatisticTypeId = 17L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.AddXP(xp).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, otherStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(otherStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToAssignPoints()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 9

                Dim statisticType As New Mock(Of ICharacterStatisticType)
                worldData.Setup(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.Setup(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.AssignPoint(statisticType.Object)

                statisticType.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherOrNotAQuestCanBeAccepted()
        WithSubject(
            Sub(worldData, id, subject)
                Const quest = Game.OldQuestType.CellarRats
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.CanAcceptQuest(quest).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, 1))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, "CharacterCanAcceptCellarRatsQuest", {1}))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherOrNotAGivenItemTypeCanBribeAGivenCharacter()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 2L
                Const itemTypeId = 14L

                Dim characterData = New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(X) X.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)

                subject.CanBeBribedWith(Game.ItemType.FromId(worldData.Object, itemTypeId)).ShouldBeFalse

                characterData.Verify(Function(x) x.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId))
                characterData.VerifyNoOtherCalls()
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherAGiveCharacterCanCastAGivenSpell()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 1L
                worldData.SetupGet(Function(x) x.SpellType).Returns((New Mock(Of ISpellTypeData)).Object)
                worldData.SetupGet(Function(x) x.Events).Returns((New Mock(Of IEventData)).Object)
                Dim spellType = Game.SpellType.FromId(worldData.Object, spellTypeId)

                subject.CanCastSpell(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.SpellType.ReadCastCheck(spellTypeId))
                worldData.Verify(Function(x) x.Events.Test(It.IsAny(Of IWorldData), Nothing, id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenACharacterCanDoIntimitation()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 3

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanDoIntimidation().ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanFight()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanFight().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanGamble()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 14L

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanGamble.ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanInteract()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanInteract().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanIntimidate()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 4L

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanIntimidate.ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanLearnAGivenSpell()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 1L
                Dim spellType = Game.SpellType.FromId(worldData.Object, spellTypeId)

                worldData.SetupGet(Function(x) x.SpellType).Returns((New Mock(Of ISpellTypeData)).Object)
                worldData.SetupGet(Function(x) x.CharacterSpell).Returns((New Mock(Of ICharacterSpellData)).Object)

                subject.CanLearn(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterSpell.Read(id, spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadMaximumLevel(spellTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanMap()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanMap.ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldContainTheCharacterMovementSubject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Movement.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveCharacterTypeFromAGivenCharacter()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)

                subject.CharacterType.ShouldNotBeNull

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveLocationFromAGivenCharacter()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadLocation(id)).Returns(locationId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)

                subject.Location.ShouldNotBeNull

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveNameFromAGivenCharacter()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                Dim characterTypeData As New Mock(Of ICharacterTypeData)
                worldData.SetupGet(Function(x) x.CharacterType).Returns(characterTypeData.Object)

                subject.Name.ShouldBeNull

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterType.ReadName(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineAGiveCharactersCurrentHPBasedOnThatCharactersMaximumHPAndWounds()
        WithSubject(
            Sub(worldData, id, subject)
                Const firstStatisticTypeId = 6L
                Const secondStatisticTypeId = 12L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CurrentHP.ShouldBe(0)

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineThatACharacterIsDeadBasedOnThatCharactersCurrentHP()
        WithSubject(
            Sub(worldData, id, subject)
                Const firstStatisticTypeId = 6L
                Const secondStatisticTypeId = 12L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.IsDead.ShouldBeTrue

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherAGivenCharacterIsTheEnemyOfAnotherCharacter()
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
            End Sub)
    End Sub
End Class
