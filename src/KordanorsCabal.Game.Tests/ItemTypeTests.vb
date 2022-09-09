Public Class ItemTypeTests
    Private Shared Sub WithExisting(itemTypeId As Long, stuffToDo As Action(Of Mock(Of IWorldData), ItemType))
        Dim worldData As New Mock(Of IWorldData)
        worldData.SetupGet(Function(x) x.ItemType).Returns((New Mock(Of IItemTypeData)).Object)
        Dim subject = ItemType.FromId(worldData.Object, itemTypeId)
        stuffToDo(worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldConstructFromWorldDataAndAnItemTypeId()
        Dim itemTypeId = 1L
        WithExisting(
            itemTypeId,
            Sub(worldData, subject)
                subject.Id.ShouldBe(itemTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldAttemptToReadTheName()
        Dim itemTypeId = 1L
        WithExisting(
            itemTypeId,
            Sub(worldData, subject)
                subject.Name.ShouldBeNull
                worldData.Verify(Function(x) x.ItemType.ReadName(itemTypeId))
            End Sub)
    End Sub
End Class
