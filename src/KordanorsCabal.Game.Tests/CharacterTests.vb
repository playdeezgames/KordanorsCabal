﻿Public Class CharacterTests
    Inherits BaseThingieTests(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
    End Sub
    <Fact>
    Sub ShouldAttemptToPerformQuestAcceptance()
        WithAnySubject(
            Sub(id, worldData, subject)
                Const locationId = 2L
                Const questId = 1L
                Const locationTypeId = 7L
                Const characterTypeId = 13L

                Dim locationData = New Mock(Of ILocationData)

                worldData.SetupGet(Function(x) x.CharacterQuest).Returns((New Mock(Of ICharacterQuestData)).Object)
                worldData.SetupGet(Function(x) x.CharacterQuestCompletion).Returns((New Mock(Of ICharacterQuestCompletionData)).Object)
                worldData.SetupGet(Function(x) x.Location).Returns(locationData.Object)
                locationData.Setup(Function(x) x.ReadForLocationType(It.IsAny(Of Long))).Returns(New List(Of Long) From {locationId})
                worldData.SetupGet(Function(x) x.CharacterTypeInitialStatistic).Returns((New Mock(Of ICharacterTypeInitialStatisticData)).Object)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.AcceptQuest(Quest.CellarRats)

                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questId))
                worldData.Verify(Sub(x) x.CharacterQuest.Write(id, questId))
                worldData.Verify(Sub(x) x.CharacterQuestCompletion.Read(id, questId))
                worldData.Verify(Function(x) x.Location.ReadForLocationType(locationTypeId))
                worldData.Verify(Function(x) x.CharacterTypeInitialStatistic.ReadAllForCharacterType(characterTypeId))
                worldData.Verify(Function(x) x.Character.Create(characterTypeId, locationId))
                locationData.VerifyNoOtherCalls()
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToChangeValuesAssociatedWithCurrentMP()
        WithAnySubject(
            Sub(id, worldData, subject)
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
        WithAnySubject(
            Sub(id, worldData, subject)
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
        WithAnySubject(
            Sub(id, worldData, subject)
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
        WithAnySubject(
            Sub(id, worldData, subject)
                Const quest = Game.Quest.CellarRats
                Const questId = CType(quest, Long)

                worldData.SetupGet(Function(x) x.CharacterQuest).Returns((New Mock(Of ICharacterQuestData)).Object)

                subject.CanAcceptQuest(quest).ShouldBeTrue

                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherOrNotAGivenItemTypeCanBribeAGivenCharacter()
        WithAnySubject(
            Sub(id, worldData, subject)
                Const characterTypeId = 2L
                Const itemType = OldItemType.AirShard
                Const itemTypeId = CType(itemType, Long)

                Dim characterData = New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(X) X.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)

                subject.CanBeBribedWith(itemType).ShouldBeFalse

                characterData.Verify(Function(x) x.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId))
                characterData.VerifyNoOtherCalls()
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherAGiveCharacterCanCastAGivenSpell()
        WithAnySubject(
            Sub(id, worldData, subject)
                Const spellType = Game.SpellType.Purify
                Const statisticTypeId = 8L
                Const otherStatisticTypeId = 15L

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanCastSpell(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, otherStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(otherStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenACharacterCanDoIntimitation()
        WithAnySubject(
            Sub(id, worldData, subject)
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
        WithAnySubject(
            Sub(id, worldData, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanFight().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanGamble()
        WithAnySubject(
            Sub(id, worldData, subject)
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
        WithAnySubject(
            Sub(id, worldData, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanInteract().ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanIntimidate()
        WithAnySubject(
            Sub(id, worldData, subject)
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
        WithAnySubject(
            Sub(id, worldData, subject)
                Const spellType = Game.SpellType.HolyBolt
                Const spellTypeId = CType(spellType, Long)
                Const statisticTypeId = 5L

                worldData.SetupGet(Function(x) x.CharacterSpell).Returns((New Mock(Of ICharacterSpellData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanLearn(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterSpell.Read(id, spellTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanMap()
        WithAnySubject(
            Sub(id, worldData, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanMap.ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldContainTheCharacterMovementSubject()
        WithAnySubject(
            Sub(id, worldData, subject)
                subject.Movement.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveCharacterTypeFromAGivenCharacter()
        WithAnySubject(
            Sub(id, worldData, subject)
                Const characterTypeId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)

                subject.CharacterType.ShouldNotBeNull

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
            End Sub)
    End Sub
End Class
