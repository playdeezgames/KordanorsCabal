Public Class ArmorShould
    Private Sub WithSubject(stuffToDo As Action(Of Mock(Of IWorldData), Long, IArmor))
        Const id = 1L
        Dim worldData As New Mock(Of IWorldData)
        Dim subject As IArmor = Game.Armor.FromId(worldData.Object, id)
        stuffToDo(worldData, id, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub have_an_is_armor_property()
        WithSubject(
            Sub(worldData, itemId, item)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                item.IsArmor.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(itemId))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 4L))
            End Sub)
    End Sub

End Class
