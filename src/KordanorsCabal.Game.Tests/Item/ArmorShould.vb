Public Class ArmorShould
    Inherits ThingieShould(Of IArmor)
    Public Sub New()
        MyBase.New(AddressOf Armor.FromId)
    End Sub
    <Fact>
    Sub have_an_is_armor_property()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.IsArmor.ShouldBeFalse
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, 32L))
            End Sub)
    End Sub
    <Fact>
    Sub have_defense_dice()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.ItemStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.DefendDice.ShouldBe(0)
                worldData.Verify(Function(x) x.ItemStatistic.Read(id, 32L))
            End Sub)
    End Sub
End Class
