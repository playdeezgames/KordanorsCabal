Public Class CharacterTests
    Inherits BaseThingieTests(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
    End Sub
    <Fact>
    Sub ShouldPerformQuestAcceptance()
        WithAnySubject(
            Sub(id, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterQuest).Returns((New Mock(Of ICharacterQuestData)).Object)
                worldData.SetupGet(Function(x) x.CharacterQuestCompletion).Returns((New Mock(Of ICharacterQuestCompletionData)).Object)
                Dim locationData = New Mock(Of ILocationData)
                worldData.SetupGet(Function(x) x.Location).Returns(locationData.Object)
                Const locationId = 2L
                locationData.Setup(Function(x) x.ReadForLocationType(It.IsAny(Of Long))).Returns(New List(Of Long) From {locationId})
                worldData.SetupGet(Function(x) x.CharacterTypeInitialStatistic).Returns((New Mock(Of ICharacterTypeInitialStatisticData)).Object)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)
                subject.AcceptQuest(Quest.CellarRats)
                worldData.Verify(Function(x) x.CharacterQuest.Read(1, 1)) 'TODO: magic numbers!
                worldData.Verify(Sub(x) x.CharacterQuest.Write(1, 1)) 'TODO: magic numbers!
                worldData.Verify(Sub(x) x.CharacterQuestCompletion.Read(1, 1)) 'TODO: magic numbers!
                worldData.Verify(Function(x) x.Location.ReadForLocationType(7)) 'TODO: magic numbers!
                worldData.Verify(Function(x) x.CharacterTypeInitialStatistic.ReadAllForCharacterType(13)) 'TODO: magic numbers!
                worldData.Verify(Function(x) x.Character.Create(13, 2)) 'TODO: magic numbers!
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToChangeValuesAssociatedWithCurrentMP()
        WithAnySubject(
            Sub(id, worldData, subject)
                Dim stress = 2L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.AddStress(stress)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(1, 13)) 'TODO: magic numbers
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(13)) 'TODO: magic numbers
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToAddXP()
        WithAnySubject(
            Sub(id, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                Dim xp = 2L
                subject.AddXP(xp).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterStatistic.Read(1, 16)) 'TODO: magic numbers!
                worldData.Verify(Function(x) x.CharacterStatistic.Read(1, 17)) 'TODO: magic numbers!
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(16)) 'TODO: magic numbers!
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(17)) 'TODO: magic numbers!
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToAssignPoints()
        WithAnySubject(
            Sub(id, worldData, subject)
                worldData.Setup(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.Setup(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                Dim statisticType As New Mock(Of ICharacterStatisticType)
                subject.AssignPoint(statisticType.Object)
                statisticType.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(1, 9)) 'TODO: magic numbers!
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(9)) 'TODO: magic numbers!
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherOrNotAQuestCanBeAccepted()
        WithAnySubject(
            Sub(id, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterQuest).Returns((New Mock(Of ICharacterQuestData)).Object)
                Dim quest = Game.Quest.CellarRats
                subject.CanAcceptQuest(quest).ShouldBeTrue
                worldData.Verify(Function(x) x.CharacterQuest.Read(1, 1)) 'TODO: magic numbers!
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhetherOrNotAGivenItemTypeCanBribeAGivenCharacter()
        WithAnySubject(
            Sub(id, worldData, subject)
                Dim itemType = OldItemType.AirShard
                Dim characterData = New Mock(Of ICharacterData)
                Const characterTypeId = 2L
                characterData.Setup(Function(x) x.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)
                worldData.SetupGet(Function(X) X.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)
                subject.CanBeBribedWith(itemType).ShouldBeFalse
                characterData.Verify(Function(x) x.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, 14L)) 'TODO: magic numbers
            End Sub)
    End Sub
End Class
