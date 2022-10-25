Public Class CharacterManaShould
    Inherits ThingieShould(Of ICharacterMana)
    Sub New()
        MyBase.New(Function(w, i) CharacterMana.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub have_maximum_mana()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.MaximumMana.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 8))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_current_mana()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.CurrentMana.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 8))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(8))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 15))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(15))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub do_fatigue()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoFatigue(wear)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 15))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(15))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
