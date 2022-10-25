Public Class DurabilityShould
    Inherits ThingieShould(Of IDurability)

    Public Sub New()
        MyBase.New(AddressOf Durability.FromId)
    End Sub

    <Fact>
    Sub have_durability()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                subject.Current.ShouldBe(1L)
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 33L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, 28L))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(28L))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_durability()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Maximum.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 33L))
            End Sub)
    End Sub
    <Fact>
    Sub reduce_durability()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                subject.Reduce(1L)
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 33L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, 28L))
                worldData.Verify(Sub(x) x.ItemStatistic.Write(id, 28L, 1L))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(28L))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_broken_flag()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                subject.IsBroken.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 33L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, 28L))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(28))
            End Sub)
    End Sub
End Class
