Public Class UsageShould
    Inherits ThingieShould(Of IUsage)
    Sub New()
        MyBase.New(AddressOf Usage.FromId)
    End Sub
    <Fact>
    Sub have_can_use_test()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 2L
                worldData.Setup(Function(x) x.ItemEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CanUse(Character.FromId(worldData.Object, characterId)).ShouldBeFalse
                worldData.Verify(Function(x) x.ItemEvent.Read(id, 2L))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_is_consumed_property()
        WithSubject(
            Sub(worldData, id, subject)
                Const singleUse = 37L
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.IsConsumed.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, singleUse))
            End Sub)
    End Sub
    <Fact>
    Sub decay()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Decay()
                worldData.Verify(Function(x) x.ItemEvent.Read(id, 4L))
            End Sub)
    End Sub
    <Fact>
    Sub purify()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Purify()
                worldData.Verify(Function(x) x.ItemEvent.Read(id, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub allow_use_by_a_character()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterId = 3L
                worldData.Setup(Function(x) x.ItemEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Use(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.ItemEvent.Read(id, 3L))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub

End Class
