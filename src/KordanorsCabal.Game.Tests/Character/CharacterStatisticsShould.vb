Public Class CharacterStatisticsShould
    Inherits ThingieShould(Of ICharacterStatistics)
    Sub New()
        MyBase.New(Function(w, i) CharacterStatistics.FromCharacter(w, Character.FromId(w, i)))
    End Sub
    <Fact>
    Sub set_statistic()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                Const statisticValue = 3L
                worldData.Setup(Function(x) x.StatisticType.ReadMinimumValue(It.IsAny(Of Long))).Returns(0)
                worldData.Setup(Function(x) x.StatisticType.ReadMaximumValue(It.IsAny(Of Long))).Returns(10)
                worldData.Setup(Sub(x) x.CharacterStatistic.Write(It.IsAny(Of Long), It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.SetStatistic(StatisticType.FromId(worldData.Object, statisticTypeId), statisticValue)
                worldData.Verify(Function(x) x.StatisticType.ReadMinimumValue(statisticTypeId))
                worldData.Verify(Function(x) x.StatisticType.ReadMaximumValue(statisticTypeId))
                worldData.Verify(Sub(x) x.CharacterStatistic.Write(id, statisticTypeId, statisticValue))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub change_statistic()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                Const delta = 3L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.ChangeStatistic(StatisticType.FromId(worldData.Object, statisticTypeId), delta)
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
    <Fact>
    Sub get_statistic()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticTypeId = 2L
                worldData.Setup(Function(x) x.CharacterStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                worldData.Setup(Function(x) x.StatisticType.ReadDefaultValue(It.IsAny(Of Long)))
                subject.GetStatistic(StatisticType.FromId(worldData.Object, statisticTypeId)).ShouldBeNull
                worldData.Verify(Function(x) x.CharacterStatistic.Read(id, statisticTypeId))
                worldData.Verify(Function(x) x.StatisticType.ReadDefaultValue(statisticTypeId))
                worldData.Verify(Function(x) x.Player.Read())
            End Sub)
    End Sub
End Class
