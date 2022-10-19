Public Class CharacterMentalCombatShould
    Inherits ThingieShould(Of ICharacterMentalCombat)
    Sub New()
        MyBase.New(Function(w, i) CharacterMentalCombat.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub change_current_mp()
        WithSubject(
            Sub(worldData, id, subject)
                Const stress = 2L
                Const statisticTypeId = 13L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.AddStress(stress)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub roll_influence()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollInfluence().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 3))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(3))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub roll_willpower()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.RollWillpower().ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 4))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(4))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub do_intimidation()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoIntimidation()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 3))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(3))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_current_mp()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.CurrentMP.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 7))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(7))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 13))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(13))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_is_demoralized()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.IsDemoralized.ShouldBeTrue
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 4))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 7))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 13))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(4))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(7))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(13))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_can_do_intimidation()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 3

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanDoIntimidation().ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_can_intimidate()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 4L

                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.CanIntimidate.ShouldBeFalse

                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
