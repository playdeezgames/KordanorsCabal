Namespace KordanorsCabal.Game.Tests
    Public Class CharacterTypeTests
        Private Shared Sub WithSpecificCharacterType(characterTypeId As Long, stuffToDo As Action(Of Mock(Of IWorldData), ICharacterType))
            Dim worldData As New Mock(Of IWorldData)
            worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
            worldData.SetupGet(Function(x) x.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)
            Dim subject As ICharacterType = CharacterType.FromId(worldData.Object, characterTypeId)
            stuffToDo(worldData, subject)
            worldData.VerifyNoOtherCalls()
        End Sub
        Private Shared Sub WithAnyCharacterType(stuffToDo As Action(Of Long, Mock(Of IWorldData), ICharacterType))
            Dim characterTypeId = 1
            WithSpecificCharacterType(
                characterTypeId,
                Sub(worldData, characterType)
                    stuffToDo(characterTypeId, worldData, characterType)
                End Sub)
        End Sub
        <Fact>
        Sub ShouldConstructFromWorldDataAndACharacterTypeId()
            WithAnyCharacterType(
                Sub(characterTypeId, worldData, subject)
                    subject.Id.ShouldBe(characterTypeId)
                End Sub)
        End Sub
        <Fact>
        Sub ShouldQueryForUndeadStatusOfAGivenCharacterType()
            WithAnyCharacterType(
                Sub(characterTypeId, worldData, subject)
                    Dim actual = subject.IsUndead
                    actual.ShouldBeFalse
                    worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId), Times.Once)
                End Sub)
        End Sub
        <Fact>
        Sub ShouldQueryForTheAbilityToBribeAGivenCharacterTypeWitAGivenItemType()
            WithAnyCharacterType(
                Sub(characterTypeId, worldData, subject)
                    Dim itemTypeId = OldItemType.AirShard
                    Dim actual = subject.CanBeBribedWith(itemTypeId)
                    actual.ShouldBeFalse
                    worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId), Times.Once)
                End Sub)
        End Sub
    End Class
End Namespace

