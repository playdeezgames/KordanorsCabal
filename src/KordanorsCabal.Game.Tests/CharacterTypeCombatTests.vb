Public Class CharacterTypeCombatTests
    Private Shared Sub WithAnyCharacterType(stuffToDo As Action(Of Long, Mock(Of IWorldData), ICharacterTypeCombat))
        Dim characterTypeId = 1
        Dim worldData As New Mock(Of IWorldData)
        Dim subject As ICharacterTypeCombat = CharacterTypeCombat.FromId(worldData.Object, characterTypeId)
        stuffToDo(characterTypeId, worldData, subject)
        worldData.VerifyNoOtherCalls()
    End Sub
    <Fact>
    Sub ShouldQueryForTheAbilityToBribeAGivenCharacterTypeWitAGivenItemType()
        WithAnyCharacterType(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterTypeBribe).Returns((New Mock(Of ICharacterTypeBribeData)).Object)
                Dim itemTypeId = OldItemType.AirShard
                Dim actual = subject.CanBeBribedWith(itemTypeId)
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterTypeBribe.Read(characterTypeId, itemTypeId), Times.Once)
            End Sub)
    End Sub

End Class
