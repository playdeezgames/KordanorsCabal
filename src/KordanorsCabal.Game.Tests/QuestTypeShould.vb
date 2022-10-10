Public Class QuestTypeShould
    Inherits ThingieShould(Of IQuestType)
    Public Sub New()
        MyBase.New(AddressOf QuestType.FromId)
    End Sub
    <Fact>
    Sub have_can_accept()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.QuestType.ReadCanAcceptEventName(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.CanAccept(Character.FromId(worldData.Object, characterId)).ShouldBeFalse
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(id))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {characterId}))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_complete()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.QuestType.ReadCanCompleteEventName(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.CanComplete(Character.FromId(worldData.Object, characterId)).ShouldBeFalse
                worldData.Verify(Function(x) x.QuestType.ReadCanCompleteEventName(id))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {characterId}))
            End Sub)
    End Sub
    <Fact>
    Sub have_accept()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.QuestType.ReadCanAcceptEventName(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.Accept(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(id))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {characterId}))
            End Sub)
    End Sub
    <Fact>
    Sub have_complete()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Complete(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.CharacterQuest.Read(characterId, id))
            End Sub)
    End Sub
End Class
