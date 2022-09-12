Public Class CharacterTypeTests
    Inherits BaseThingieTests(Of ICharacterType)

    Public Sub New()
        MyBase.New(AddressOf CharacterType.FromId)
    End Sub

    <Fact>
    Sub ShouldConstructFromWorldDataAndACharacterTypeId()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                subject.Id.ShouldBe(characterTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForUndeadStatusOfAGivenCharacterType()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.IsUndead
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub ShouldQueryForNameOfAGivenCharacterType()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.Name
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterType.ReadName(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveSpawningSubobjectFromAGivenCharacterType()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                subject.Spawning.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub ShouldRetrieveCombatSubobjectFromAGivenCharacterType()
        WithAnySubject(
            Sub(characterTypeId, worldData, subject)
                subject.Combat.ShouldNotBeNull
            End Sub)
    End Sub
End Class


