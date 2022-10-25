Public Class Repair
    Inherits BaseThingie
    Implements IRepair

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub

    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IRepair
        Return If(id.HasValue, New Repair(worldData, id.Value), Nothing)
    End Function
    Public ReadOnly Property IsNeeded As Boolean Implements IRepair.IsNeeded
        Get
            Dim durability = Game.Durability.FromId(WorldData, Id)
            Return durability.Maximum.HasValue AndAlso durability.Current.Value < durability.Maximum.Value
        End Get
    End Property
    Public Sub Perform() Implements IRepair.Perform
        SetStatistic(1L, 0)
    End Sub
    Private Sub SetStatistic(statisticType As Long, value As Long)
        WorldData.ItemStatistic.Write(Id, statisticType, value)
    End Sub
    Public Function RepairCost(shoppeType As IShoppeType) As Long Implements IRepair.RepairCost
        Dim fullRepairPrice = shoppeType.RepairPrice(Item.FromId(WorldData, Id).ItemType)
        Dim wear = GetStatistic(StatisticType.FromId(WorldData, StatisticTypeDurability))
        Dim maximum = Durability.FromId(WorldData, Id).Maximum.Value
        Dim remainder = If(wear * fullRepairPrice Mod maximum > 0, 1, 0)
        Return wear * If(fullRepairPrice, 0) \ maximum + remainder
    End Function
    Private Function GetStatistic(statisticType As IStatisticType) As Long
        Return If(WorldData.ItemStatistic.Read(Id, statisticType.Id), statisticType.DefaultValue.Value)
    End Function
End Class
