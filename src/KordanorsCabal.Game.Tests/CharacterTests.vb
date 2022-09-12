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
                worldData.Verify(Function(x) x.CharacterQuest.Read(1, 1))
                worldData.Verify(Sub(x) x.CharacterQuest.Write(1, 1))
                worldData.Verify(Sub(x) x.CharacterQuestCompletion.Read(1, 1))
                worldData.Verify(Function(x) x.Location.ReadForLocationType(7))
                worldData.Verify(Function(x) x.CharacterTypeInitialStatistic.ReadAllForCharacterType(13))
                worldData.Verify(Function(x) x.Character.Create(13, 2))
            End Sub)
    End Sub
End Class
