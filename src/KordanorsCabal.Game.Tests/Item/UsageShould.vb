Public Class UsageShould
    Inherits ThingieShould(Of IUsage)
    Sub New()
        MyBase.New(AddressOf Usage.FromId)
    End Sub
    <Fact>
    Sub have_can_use_test()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                Const characterId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.CanUse(Character.FromId(worldData.Object, characterId)).ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 2L))
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
    Sub allow_use_by_a_character()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                Const characterId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeEvent.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Use(Character.FromId(worldData.Object, characterId))
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeEvent.Read(itemTypeId, 3L))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub

End Class
