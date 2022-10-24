Public Class ItemTypeCombatShould
    Inherits ThingieShould(Of IItemTypeCombat)

    Public Sub New()
        MyBase.New(Function(w, i) ItemTypeCombat.FromId(w, i))
    End Sub
    <Fact>
    Sub item_types_have_attack_dice()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.AttackDice.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 30))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_maximum_damage()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.MaximumDamage.ShouldBeNull
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 31))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_have_defend_dice()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.DefendDice.ShouldBe(0L)
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 32))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_is_weapon_determiner()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.IsWeapon.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 30))
            End Sub)
    End Sub
    <Fact>
    Sub item_types_has_is_armor_determiner()
        WithSubject(
            Sub(worldData, itemTypeId, subject)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.IsArmor.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 32))
            End Sub)
    End Sub
End Class
