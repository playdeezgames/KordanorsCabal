Public Class CharacterSpellbookShould
    Inherits ThingieShould(Of ICharacterSpellbook)
    Sub New()
        MyBase.New(Function(w, i) CharacterSpellbook.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub roll_spell_dice()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                worldData.Setup(Function(x) x.CharacterSpell.ReadForCharacter(It.IsAny(Of Long)))
                subject.RollSpellDice(SpellType.FromId(worldData.Object, spellTypeId)).ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterSpell.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub roll_power()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollPower().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 5))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(5))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub learn()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                worldData.Setup(Function(x) x.CharacterSpell.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.SpellType.ReadMaximumLevel(It.IsAny(Of Long)))
                subject.Learn(SpellType.FromId(worldData.Object, spellTypeId))
                worldData.Verify(Function(x) x.CharacterSpell.Read(id, spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadMaximumLevel(spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadName(spellTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_spells()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterSpell.ReadForCharacter(It.IsAny(Of Long)))
                subject.Spells.ShouldBeEmpty
                worldData.Verify(Function(x) x.CharacterSpell.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub cast()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 2L
                worldData.Setup(Function(x) x.SpellType.ReadCastCheck(It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.Events.Test(It.IsAny(Of IWorldData), It.IsAny(Of String), It.IsAny(Of Long())))
                subject.Cast(SpellType.FromId(worldData.Object, spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadCastCheck(spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadName(spellTypeId))
                worldData.Verify(Function(x) x.Events.Test(worldData.Object, Nothing, {id}))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_has_spells()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterSpell.ReadForCharacter(It.IsAny(Of Long)))
                subject.HasSpells.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterSpell.ReadForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_can_learn()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 1L
                Dim spellType = Game.SpellType.FromId(worldData.Object, spellTypeId)

                worldData.SetupGet(Function(x) x.SpellType).Returns((New Mock(Of ISpellTypeData)).Object)
                worldData.SetupGet(Function(x) x.CharacterSpell).Returns((New Mock(Of ICharacterSpellData)).Object)

                subject.CanLearn(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterSpell.Read(id, spellTypeId))
                worldData.Verify(Function(x) x.SpellType.ReadMaximumLevel(spellTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_can_cast_spell()
        WithSubject(
            Sub(worldData, id, subject)
                Const spellTypeId = 1L
                worldData.SetupGet(Function(x) x.SpellType).Returns((New Mock(Of ISpellTypeData)).Object)
                worldData.SetupGet(Function(x) x.Events).Returns((New Mock(Of IEventData)).Object)
                Dim spellType = Game.SpellType.FromId(worldData.Object, spellTypeId)

                subject.CanCastSpell(spellType).ShouldBeFalse

                worldData.Verify(Function(x) x.SpellType.ReadCastCheck(spellTypeId))
                worldData.Verify(Function(x) x.Events.Test(It.IsAny(Of IWorldData), Nothing, id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
