Public Class CharacterAdvancementShould
    Inherits ThingieShould(Of ICharacterAdvancement)
    Sub New()
        MyBase.New(Function(w, i) CharacterAdvancement.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub add_xp()
        WithSubject(
            Sub(worldData, id, subject)
                Const xp = 2L
                Const statisticTypeId = 16L
                Const otherStatisticTypeId = 17L
                worldData.SetupGet(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.SetupGet(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)
                subject.AddXP(xp).ShouldBeFalse
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, otherStatisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(otherStatisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub assign_points()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 9

                Dim statisticType As New Mock(Of IStatisticType)
                worldData.Setup(Function(x) x.CharacterStatistic).Returns((New Mock(Of ICharacterStatisticData)).Object)
                worldData.Setup(Function(x) x.CharacterStatisticType).Returns((New Mock(Of ICharacterStatisticTypeData)).Object)

                subject.AssignPoint(statisticType.Object)

                statisticType.VerifyNoOtherCalls()
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub have_is_fully_assigned()
        WithSubject(
            Sub(worldData, id, subject)
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.CharacterStatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.IsFullyAssigned.ShouldBeTrue
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, 9))
                worldData.Verify(Function(x) x.CharacterStatisticType.ReadDefaultValue(9))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
