Public Class CharacterQuestShould
    Inherits ThingieShould(Of ICharacterQuest)

    Public Sub New()
        MyBase.New(Function(w, i) CharacterQuest.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub can_accept_quests()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 1L
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                worldData.Setup(Function(x) x.QuestType.ReadCanAcceptEventName(It.IsAny(Of Long)))
                subject.Accept(QuestType.FromId(worldData.Object, questTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {1L}))
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(questTypeId))
                worldData.Verify(Function(x) x.Player.Read())
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
                subject.CanAccept(QuestType.FromId(worldData.Object, questTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {1}))
                worldData.Verify(Function(x) x.QuestType.ReadCanAcceptEventName(questTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub complete_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Complete(QuestType.FromId(worldData.Object, questTypeId))
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_has_quest()
        WithSubject(
            Sub(worldData, id, subject)
                Const questTypeId = 2L
                worldData.Setup(Function(x) x.CharacterQuest.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Has(QuestType.FromId(worldData.Object, questTypeId)).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterQuest.Read(id, questTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
