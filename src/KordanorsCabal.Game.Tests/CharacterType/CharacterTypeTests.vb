Public Class CharacterTypeTests
    Inherits ThingieShould(Of ICharacterType)

    Public Sub New()
        MyBase.New(AddressOf CharacterType.FromId)
    End Sub

    <Fact>
    Sub character_types_store_character_type_ids()
        WithSubject(
            Sub(worldData, characterTypeId, subject)
                subject.Id.ShouldBe(characterTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub character_types_have_an_undead_flag_fetched_from_the_data_store()
        WithSubject(
            Sub(worldData, characterTypeId, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.IsUndead
                actual.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId), Times.Once)
            End Sub)
    End Sub
    <Fact>
    Sub character_types_have_names_fetched_from_the_data_store()
        WithSubject(
            Sub(worldData, characterTypeId, subject)
                worldData.SetupGet(Function(x) x.CharacterType).Returns((New Mock(Of ICharacterTypeData)).Object)
                Dim actual = subject.Name
                actual.ShouldBeNull
                worldData.Verify(Function(x) x.CharacterType.ReadName(characterTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub character_types_contain_spawning_subobjects()
        WithSubject(
            Sub(worldData, characterTypeId, subject)
                subject.Spawning.Id.ShouldBe(characterTypeId)
            End Sub)
    End Sub
    <Fact>
    Sub character_types_contain_combat_subobjects()
        WithSubject(
            Sub(worldData, characterTypeId, subject)
                subject.Combat.Id.ShouldBe(characterTypeId)
            End Sub)
    End Sub
End Class


