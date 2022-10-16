Public Class CharacterShould
    Inherits ThingieShould(Of ICharacter)
    Sub New()
        MyBase.New(AddressOf Character.FromId)
    End Sub
    <Fact>
    Sub have_movement_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Movement.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_character_type()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 1L
                Dim characterData As New Mock(Of ICharacterData)
                characterData.Setup(Function(x) x.ReadCharacterType(id)).Returns(characterTypeId)
                worldData.SetupGet(Function(x) x.Character).Returns(characterData.Object)

                subject.CharacterType.ShouldNotBeNull

                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_mode()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.Player.ReadPlayerMode()).Returns(0)
                subject.Mode.ShouldBe(Game.Constants.PlayerModes.None)
                worldData.Verify(Function(x) x.Player.ReadPlayerMode())
            End Sub)
    End Sub
    <Fact>
    Sub destroy()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Sub(x) x.Character.Clear(It.IsAny(Of Long)))
                subject.Destroy()
                worldData.Verify(Sub(x) x.Character.Clear(id))
            End Sub)
    End Sub
    <Fact>
    Sub enqueue_messages_without_sfx()
        WithSubject(
            Sub(worldData, id, subject)
                subject.EnqueueMessage("text")
            End Sub)
    End Sub
    <Fact>
    Sub enqueue_messages_with_sfx()
        WithSubject(
            Sub(worldData, id, subject)
                subject.EnqueueMessage(Sfx.Miss, "text")
            End Sub)
    End Sub
    <Fact>
    Sub have_quest_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Quest.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_health_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Health.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_physicalcombat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.PhysicalCombat.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_advancement_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Advancement.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_mentalcombat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.MentalCombat.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_spellbook_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Spellbook.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_mana_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Mana.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_statistics_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Statistics.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_items_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Items.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_repair_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Repair.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_encumbrance_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Encumbrance.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_statuses_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Statuses.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_interactions_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Interaction.ShouldNotBeNull
            End Sub)
    End Sub
    <Fact>
    Sub have_equipment_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Equipment.ShouldNotBeNull
            End Sub)
    End Sub
End Class
