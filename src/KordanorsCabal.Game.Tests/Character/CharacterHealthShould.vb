Public Class CharacterHealthShould
    Inherits ThingieShould(Of ICharacterHealth)
    Sub New()
        MyBase.New(AddressOf CharacterHealth.FromId)
    End Sub
    <Fact>
    Sub have_needs_healing()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.NeedsHealing.ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 12))
                worldData.Verify(Function(x) x.CharacterEquipSlot.ReadItemsForCharacter(id))
            End Sub)
    End Sub
    <Fact>
    Sub have_maximum_hp()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.Maximum.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 6))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(6))
            End Sub)
    End Sub
    <Fact>
    Sub have_is_dead()
        WithSubject(
            Sub(worldData, id, subject)
                Const firstStatisticTypeId = 6L
                Const secondStatisticTypeId = 12L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.IsDead.ShouldBeTrue

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticTypeId))
            End Sub)
    End Sub
    <Fact>
    Sub heal()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadMinimumValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadMaximumValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Sub(x) x.CharacterStatistic.Write(It.IsAny(Of Long), It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.Heal()
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadMinimumValue(12))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadMaximumValue(12))
                worldData.Verify(Sub(x) x.CharacterStatistic.Write(id, 12, 0))
            End Sub)
    End Sub
    <Fact>
    Sub have_current_hp()
        WithSubject(
            Sub(worldData, id, subject)
                Const firstStatisticTypeId = 6L
                Const secondStatisticTypeId = 12L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.Current.ShouldBe(0)

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, secondStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(firstStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(secondStatisticTypeId))
            End Sub)
    End Sub
End Class
