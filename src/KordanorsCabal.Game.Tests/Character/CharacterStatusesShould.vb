﻿Public Class CharacterStatusesShould
    Inherits ThingieShould(Of ICharacterStatuses)
    Sub New()
        MyBase.New(Function(w, i) CharacterStatuses.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub have_is_undead()
        WithSubject(
            Sub(worldData, id, subject)
                Const characterTypeId = 2L
                worldData.Setup(Function(x) x.Character.ReadCharacterType(It.IsAny(Of Long))).Returns(characterTypeId)
                worldData.Setup(Function(x) x.CharacterType.ReadIsUndead(It.IsAny(Of Long)))
                subject.IsUndead.ShouldBeFalse
                worldData.Verify(Function(x) x.Character.ReadCharacterType(id))
                worldData.Verify(Function(x) x.CharacterType.ReadIsUndead(characterTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_hunger()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Hunger.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 20))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(20))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_highness()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Highness.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 19))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(19))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_food_poisoning()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.FoodPoisoning.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 21))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(21))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_drunkenness()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Drunkenness.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 18))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(18))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_chafing()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(It.IsAny(Of Long)))
                subject.Chafing.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 22))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(22))
                worldData.Verify(Function(x) x.Character.EquipSlot.ReadItemsForCharacter(id))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_money()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.Money.ShouldBe(0)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 14))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(14))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub do_immobilization()
        WithSubject(
            Sub(worldData, id, subject)
                Const wear = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.DoImmobilization(wear)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 23))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(23))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
