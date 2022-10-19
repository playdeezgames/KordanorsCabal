Public Class CharacterMovementTests
    Inherits ThingieShould(Of ICharacterMovement)
    Sub New()
        MyBase.New(Function(w, i) CharacterMovement.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub have_can_map()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                subject.CanMap.ShouldBeFalse

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_location()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadLocation(id)).Returns(locationId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)

                subject.Location.ShouldNotBeNull

                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_direction()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Player.ReadDirection()).Returns(directionId)
                subject.Direction.Id.ShouldBe(directionId)
                worldData.Verify(Function(x) x.Player.ReadDirection())
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_has_visited()
        WithSubject(
            Sub(worldData, id, subject)
                Const locationId = 2L
                worldData.Setup(Function(x) x.CharacterLocation.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.HasVisited(Location.FromId(worldData.Object, locationId))
                worldData.Verify(Function(x) x.CharacterLocation.Read(id, locationId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_can_move_forward()
        WithSubject(
            Sub(worldData, id, subject)
                Const directionId = 2L
                worldData.Setup(Function(x) x.Player.ReadDirection()).Returns(directionId)
                worldData.Setup(Function(x) x.Inventory.ReadForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.InventoryItem.ReadItems(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Character.ReadLocation(It.IsAny(Of Long)))
                subject.CanMoveForward()
                worldData.Verify(Function(x) x.Player.ReadDirection())
                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(0))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 1))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 24))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 25))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(1))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(24))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(25))
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    Private Sub WithMovementSubject(stuffToDo As Action(Of IDirection, Mock(Of IWorldData), ICharacterMovement))
        WithSubject(
            Sub(worldData, id, subject)
                Dim direction As New Mock(Of IDirection)
                Dim createdInventoryId = 1L
                Dim inventory = New Mock(Of IInventoryData)
                Const firstStatisticId = 24
                Const secondStatisticId = 25
                Const thirdStatisticId = 1
                inventory.Setup(Function(x) x.CreateForCharacter(It.IsAny(Of Long))).Returns(createdInventoryId)
                worldData.SetupGet(Function(x) x.Inventory).Returns(inventory.Object)
                worldData.SetupGet(Function(x) x.InventoryItem).Returns((New Mock(Of IInventoryItemData)).Object)
                worldData.SetupGet(Function(x) x.Character.EquipSlot).Returns((New Mock(Of ICharacterEquipSlotData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                worldData.SetupGet(Function(x) x.Character).Returns((New Mock(Of ICharacterData)).Object)

                stuffToDo(direction.Object, worldData, subject)

                worldData.Verify(Function(x) x.Inventory.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Inventory.CreateForCharacter(id))
                worldData.Verify(Function(x) x.InventoryItem.ReadItems(createdInventoryId))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, thirdStatisticId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(thirdStatisticId))
                worldData.Verify(Function(x) x.Character.ReadLocation(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub characters_determine_whether_they_can_move_in_a_given_direction()
        WithMovementSubject(
            Sub(direction, worldData, subject)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.CanMove(direction).ShouldBeFalse
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    Private Sub WithRelativeMovementSubject(stuffToDo As Action(Of Long, Mock(Of IDirectionData), Mock(Of IWorldData), ICharacterMovement))
        WithMovementSubject(
            Sub(direction, worldData, subject)
                Dim playerData = New Mock(Of IPlayerData)
                Dim directionData = New Mock(Of IDirectionData)
                Const directionId = 1L
                playerData.Setup(Function(x) x.ReadDirection()).Returns(directionId)
                worldData.SetupGet(Function(x) x.Player).Returns(playerData.Object)
                worldData.SetupGet(Function(x) x.Direction).Returns(directionData.Object)
                stuffToDo(directionId, directionData, worldData, subject)
                playerData.Verify(Function(x) x.ReadDirection)
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub characters_determine_whether_they_can_move_backward()
        WithRelativeMovementSubject(
            Sub(directionId, directionData, worldData, subject)
                Const oppositeDirectionId = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                directionData.Setup(Function(x) x.ReadOpposite(directionId)).Returns(oppositeDirectionId)

                subject.CanMoveBackward.ShouldBeFalse

                directionData.Verify(Function(x) x.ReadOpposite(directionId))
            End Sub)
    End Sub
    <Fact>
    Sub characters_determine_whether_they_can_move_left()
        WithRelativeMovementSubject(
            Sub(directionId, directionData, worldData, subject)
                Const previousDirectionId = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                directionData.Setup(Function(x) x.ReadPrevious(directionId)).Returns(previousDirectionId)

                subject.CanMoveLeft.ShouldBeFalse

                directionData.Verify(Function(x) x.ReadPrevious(directionId))
            End Sub)
    End Sub
    <Fact>
    Sub characters_can_determine_whether_they_can_move_right()

        WithRelativeMovementSubject(
            Sub(directionId, directionData, worldData, subject)
                Const nextDirectionId = 2L
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                directionData.Setup(Function(x) x.ReadNext(directionId)).Returns(nextDirectionId)

                subject.CanMoveRight.ShouldBeFalse

                directionData.Verify(Function(x) x.ReadNext(directionId))
            End Sub)
    End Sub
    <Fact>
    Sub characters_can_move_in_a_given_directions()
        WithMovementSubject(
            Sub(direction, worldData, subject)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Move(direction)
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
