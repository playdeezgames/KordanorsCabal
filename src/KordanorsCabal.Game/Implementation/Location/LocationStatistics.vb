Public Class LocationStatistics
    Inherits BaseThingie
    Implements ILocationStatistics

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ILocationStatistics
        Return If(id.HasValue, New LocationStatistics(worldData, id.Value), Nothing)
    End Function
    Public Function GetStatistic(statisticType As ILocationStatisticType) As Long? Implements ILocationStatistics.GetStatistic
        Return WorldData.LocationStatistic.Read(Id, statisticType.Id)
    End Function
    Sub SetStatistic(statisticType As ILocationStatisticType, statisticValue As Long?) Implements ILocationStatistics.SetStatistic
        WorldData.LocationStatistic.Write(Id, statisticType.Id, statisticValue)
    End Sub
End Class
