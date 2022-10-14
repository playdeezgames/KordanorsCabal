﻿Public Class CharacterMovementTests
    Inherits ThingieShould(Of ICharacterMovement)
    Sub New()
        MyBase.New(Function(w, i) CharacterMovement.FromId(w, Character.FromId(w, i)))
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
    Sub characters_determine_whether_they_can_move_in_a_given_direction()
        WithMovementSubject(
            Sub(direction, worldData, subject)
                subject.CanMove(direction).ShouldBeFalse
            End Sub)
    End Sub
    Private Sub WithRelativeMovementSubject(stuffToDo As Action(Of Long, Mock(Of IDirectionData), ICharacterMovement))
        WithMovementSubject(
            Sub(direction, worldData, subject)
                Dim playerData = New Mock(Of IPlayerData)
                Dim directionData = New Mock(Of IDirectionData)
                Const directionId = 1L
                playerData.Setup(Function(x) x.ReadDirection()).Returns(directionId)
                worldData.SetupGet(Function(x) x.Player).Returns(playerData.Object)
                worldData.SetupGet(Function(x) x.Direction).Returns(directionData.Object)
                stuffToDo(directionId, directionData, subject)
                playerData.Verify(Function(x) x.ReadDirection)
            End Sub)
    End Sub
    <Fact>
    Sub characters_determine_whether_they_can_move_backward()
        WithRelativeMovementSubject(
            Sub(directionId, directionData, subject)
                Const oppositeDirectionId = 2L
                directionData.Setup(Function(x) x.ReadOpposite(directionId)).Returns(oppositeDirectionId)

                subject.CanMoveBackward.ShouldBeFalse

                directionData.Verify(Function(x) x.ReadOpposite(directionId))
            End Sub)
    End Sub
    <Fact>
    Sub characters_determine_whether_they_can_move_left()
        WithRelativeMovementSubject(
            Sub(directionId, directionData, subject)
                Const previousDirectionId = 2L
                directionData.Setup(Function(x) x.ReadPrevious(directionId)).Returns(previousDirectionId)

                subject.CanMoveLeft.ShouldBeFalse

                directionData.Verify(Function(x) x.ReadPrevious(directionId))
            End Sub)
    End Sub
    <Fact>
    Sub characters_can_determine_whether_they_can_move_right()
        WithRelativeMovementSubject(
            Sub(directionId, directionData, subject)
                Const nextDirectionId = 2L
                directionData.Setup(Function(x) x.ReadNext(directionId)).Returns(nextDirectionId)

                subject.CanMoveRight.ShouldBeFalse

                directionData.Verify(Function(x) x.ReadNext(directionId))
            End Sub)
    End Sub
    <Fact>
    Sub character_movement_can_access_character_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Character.Id.ShouldBe(id)
            End Sub)
    End Sub
    <Fact>
    Sub characters_can_move_in_a_given_directions()
        WithMovementSubject(
            Sub(direction, worldData, subject)
                subject.Move(direction)
            End Sub)
    End Sub
End Class
