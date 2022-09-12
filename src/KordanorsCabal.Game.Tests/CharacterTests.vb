Public Class CharacterTests
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
                worldData.SetupGet(Function(x) x.CharacterQuest).Returns((New Mock(Of ICharacterQuestData)).Object)
                worldData.SetupGet(Function(x) x.CharacterQuestCompletion).Returns((New Mock(Of ICharacterQuestCompletionData)).Object)
                Dim locationData = New Mock(Of ILocationData)
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
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToChangeValuesAssociatedWithCurrentMP()
        WithAnySubject(
            Sub(id, worldData, subject)
                Dim stress = 2L
                Dim statisticTypeId = 13L
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
                Dim xp = 2L
                Dim statisticTypeId = 16L
                Dim otherStatisticTypeId = 17L
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
                Dim statisticTypeId = 9
                worldData.Setup(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.Setup(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                Dim statisticType As New Mock(Of ICharacterStatisticType)
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
                Dim itemType = OldItemType.AirShard
                Dim itemTypeId = CType(itemType, Long)
                Dim characterData = New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(X) X.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)
                subject.CanBeBribedWith(itemType).ShouldBeFalse
                characterData.Verify(Function(x) x.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherAGiveCharacterCanCastAGivenSpell()
        WithAnySubject(
            Sub(id, worldData, subject)
                Dim spellType = Game.SpellType.Purify
                Dim spellTypeId = CType(spellType, Long)
                Dim statisticTypeId = 8L
                Dim otherStatisticTypeId = 15L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.CanCastSpell(spellType).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, otherStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(otherStatisticTypeId))
            End Sub)
    End Sub
End Class
