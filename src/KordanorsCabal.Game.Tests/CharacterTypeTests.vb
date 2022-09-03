Namespace KordanorsCabal.Game.Tests
    Public Class CharacterTypeTests
        Private Shared Sub WithExistingCharacterType(characterTypeId As Long, stuffToDo As Action(Of Mock(Of IWorldData), CharacterType))
            Dim worldData As New Mock(Of IWorldData)
            worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
            Dim subject = CharacterType.FromId(worldData.Object, characterTypeId)
            stuffToDo(worldData, subject)
            worldData.VerifyNoOtherCalls()
        End Sub
        <Fact>
        Sub ShouldConstructFromWorldDataAndACharacterTypeId()
            Const characterTypeId = 1L
            WithExistingCharacterType(
                characterTypeId,
                Sub(worldData, subject)
                    subject.Id.ShouldBe(characterTypeId)
                End Sub)
        End Sub
        <Fact>
        Sub ShouldQueryForUndeadStatus()
            Const characterTypeId = 1L
            WithExistingCharacterType(
                characterTypeId,
                Sub(worldData, subject)
                    Dim actual = subject.IsUndead
                    actual.ShouldBeFalse
                    worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId), Times.Once)
                End Sub)
        End Sub
    End Class
End Namespace

