Public Class ArmorShould
    Inherits ThingieShould(Of IArmor)
    Public Sub New()
        MyBase.New(AddressOf Armor.FromId)
    End Sub
    <Fact>
    Sub have_an_is_armor_property()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.IsArmor.ShouldBeFalse
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 32L))
            End Sub)
    End Sub
    <Fact>
    Sub have_defense_dice()
        WithSubject(
            Sub(worldData, id, subject)
                Const itemTypeId = 2L
                worldData.Setup(Function(x) x.Item.ReadItemType(It.IsAny(Of Long))).Returns(itemTypeId)
                worldData.Setup(Function(x) x.ItemTypeStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.DefendDice.ShouldBe(0)
                worldData.Verify(Function(x) x.Item.ReadItemType(id))
                worldData.Verify(Function(x) x.ItemTypeStatistic.Read(itemTypeId, 32L))
            End Sub)
    End Sub
End Class
