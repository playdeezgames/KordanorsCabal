Public Class RepairShould
    Private Sub WithSubject(stuffToDo As Action(Of Mock(Of IWorldData), Long, IRepair))
        Const itemId = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim weapon As IRepair = Game.Repair.FromId(worldData.Object, itemId)
        stuffToDo(worldData, itemId, weapon)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub have_needs_repair_flag()
        WithSubject(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(2L)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1L)
                item.IsNeeded.ShouldBeTrue
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5L))
                worldData.Verify(Function(x) x.ItemStatistic.Read(itemId, 1L))
            End Sub)
    End Sub
    <Fact>
    Sub repair()
        WithSubject(
            Sub(worldData, itemId, item)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.Perform()
                worldData.Verify(Sub(x) x.ItemStatistic.Write(itemId, 1L, 0L))
            End Sub)
    End Sub
    <Fact>
    Sub have_repair_costs()
        WithSubject(
            Sub(worldData, id, subject)
                Const shoppeTypeId = 2L
                Const itemTypeId = 3L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemType.ReadAll())
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(1)
                subject.RepairCost(ShoppeType.FromId(worldData.Object, shoppeTypeId)).ShouldBe(0)
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemType.ReadAll())
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, 1L))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 5))
            End Sub)
    End Sub
End Class
