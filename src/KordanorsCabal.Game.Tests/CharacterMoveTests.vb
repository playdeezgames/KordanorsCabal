Public Class CharacterMoveTests
    Inherits BaseThingieTests(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
    End Sub
    Private Sub WithCommonSubject(stuffToDo As Action(Of IDirection, Mock(Of IWorldData), ICharacter))
        WithAnySubject(
            Sub(id, worldData, subject)
                Dim direction As New Mock(Of IDirection)
                Dim createdInventoryId = 1L
                Dim inventory = New Mock(Of IInventoryData)
                Const firstStatisticId = 24
                Const secondStatisticId = 25
                Const thirdStatisticId = 1
                inventory.Setup(Function(x) x.CreateForCharacter(It.IsAny(Of Long))).Returns(createdInventoryId)
                worldData.SetupGet(Function(x) x.Inventory).Returns(inventory.Object)
                worldData.SetupGet(Function(x) x.InventoryItem).Returns((New Mock(Of IInventoryItemData)).Object)
                worldData.SetupGet(Function(x) x.CharacterEquipSlot).Returns((New Mock(Of ICharacterEquipSlotData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                stuffToDo(direction.Object, worldData, subject)

                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(createdInventoryId))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, thirdStatisticId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(thirdStatisticId))
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanMoveInAGivenDirection()
        WithCommonSubject(
            Sub(direction, worldData, subject)
                subject.CanMove(direction).ShouldBeFalse
            End Sub)
    End Sub
    <Fact>
    Sub ShouldDetermineWhenAGivenCharacterCanMoveBackwards()
        WithCommonSubject(
            Sub(direction, worldData, subject)
                Dim playerData = New Mock(Of IPlayerData)
                Dim directionData = New Mock(Of IDirectionData)
                Const directionId = 1L
                Const oppositeDirectionId = 2L
                playerData.Setup(Function(x) x.ReadDirection()).Returns(directionId)
                directionData.Setup(Function(x) x.ReadOpposite(directionId)).Returns(oppositeDirectionId)
                worldData.SetupGet(Function(x) x.Player).Returns(playerData.Object)
                worldData.SetupGet(Function(x) x.Direction).Returns(directionData.Object)

                subject.CanMoveBackward.ShouldBeFalse
                playerData.Verify(Function(x) x.ReadDirection)
                directionData.Verify(Function(x) x.ReadOpposite(directionId))
            End Sub)
    End Sub
End Class
