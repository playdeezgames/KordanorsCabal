Public MustInherit Class BaseItemType
    Inherits BaseThingie
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Protected ReadOnly Property Statistic(itemTypeId As Long, statisticType As IStatisticType) As Long?
        Get
            Return WorldData.ItemTypeStatistic.Read(itemTypeId, statisticType.Id)
        End Get
    End Property
End Class
