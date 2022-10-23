Public Class LocationStatisticsShould
    Inherits ThingieShould(Of ILocationStatistics)
    Public Sub New()
        MyBase.New(AddressOf LocationStatistics.FromId)
    End Sub
    <Fact>
    Sub set_statistics()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticType = 1L
                Const statisticValue = 2L
                worldData.Setup(Sub(x) x.LocationStatistic.Write(It.IsAny(Of Long), It.IsAny(Of Long), It.IsAny(Of Long?)))
                subject.SetStatistic(LocationStatisticType.FromId(worldData.Object, statisticType), statisticValue)
                worldData.Verify(Sub(x) x.LocationStatistic.Write(id, statisticType, statisticValue))
            End Sub)
    End Sub
    <Fact>
    Sub get_statistics()
        WithSubject(
            Sub(worldData, id, subject)
                Const statisticType = 1L
                worldData.Setup(Function(x) x.LocationStatistic.Read(It.IsAny(Of Long), It.IsAny(Of Long)))
                subject.GetStatistic(LocationStatisticType.FromId(worldData.Object, statisticType)).ShouldBeNull
                worldData.Verify(Function(x) x.LocationStatistic.Read(id, statisticType))
            End Sub)
    End Sub
End Class
