Public Class DurabilityShould
    Private Sub WithDurability(stuffToDo As Action(Of Mock(Of IWorldData), Long, IDurability))
        Const itemId = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim durability As IDurability = Game.Durability.FromId(worldData.Object, itemId)
        stuffToDo(worldData, itemId, durability)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub have_durability()
        WithDurability(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.CurrentDurability.ShouldBe(1L)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_durability()
        WithDurability(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.MaximumDurability.ShouldBeNull
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
            End Sub)
    End Sub
    <Fact>
    Sub reduce_durability()
        WithDurability(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.ReduceDurability(1L)
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 1L))
                worldData.Verify(Sub(x) x.ItemStatistic.Write(itemId, 1L, 1L))
            End Sub)
    End Sub
End Class
