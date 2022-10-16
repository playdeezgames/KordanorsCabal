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
                worldData.Verify(Function(x) x.Player.Read())
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
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub destroy()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Sub(x) x.Character.Clear(It.IsAny(Of Long)))
                subject.Destroy()
                worldData.Verify(Sub(x) x.Character.Clear(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub enqueue_messages_without_sfx()
        WithSubject(
            Sub(worldData, id, subject)
                subject.EnqueueMessage(Nothing, "text")
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub enqueue_messages_with_sfx()
        WithSubject(
            Sub(worldData, id, subject)
                subject.EnqueueMessage(Sfx.Miss, "text")
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_quest_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Quest.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_health_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Health.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_physicalcombat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.PhysicalCombat.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_advancement_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Advancement.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_mentalcombat_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.MentalCombat.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_spellbook_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Spellbook.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_mana_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Mana.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_statistics_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Statistics.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_items_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Items.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_repair_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Repair.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_encumbrance_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Encumbrance.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_statuses_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Statuses.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_interactions_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Interaction.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_equipment_subobject()
        WithSubject(
            Sub(worldData, id, subject)
                subject.Equipment.ShouldNotBeNull
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
