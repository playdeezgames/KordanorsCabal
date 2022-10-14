Public Class CharacterQuestShould
    Inherits ThingieShould(Of ICharacterQuest)

    Public Sub New()
        MyBase.New(AddressOf CharacterQuest.FromId)
    End Sub
    <Fact>
    Sub can_accept_quests()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 1L
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                worldData.Setup(Function(x) x.QuestType.ReadCanAcceptEventName(It.IsAny(Of Long)))
                subject.AcceptQuest(QuestType.FromId(worldData.Object, questTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {1L}))
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(questTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_can_accept_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                worldData.Setup(Function(x) x.QuestType.ReadCanAcceptEventName(It.IsAny(Of Long)))
                subject.CanAcceptQuest(QuestType.FromId(worldData.Object, questTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {1}))
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(questTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub complete_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CompleteQuest(QuestType.FromId(worldData.Object, questTypeId))
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub have_has_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasQuest(QuestType.FromId(worldData.Object, questTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
            End Sub)
    End Sub
End Class
