Public Class Durability
    Inherits BaseThingie
    Implements IDurability
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IDurability
        Return If(id.HasValue, New Durability(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property Current As Long? Implements IDurability.Current
        Get
            If Maximum.HasValue Then
                Return Maximum.Value - GetStatistic(ItemStatisticType.FromId(WorldData, StatisticTypeDurability))
            End If
            Return Nothing
        End Get
    End Property
    Private Function GetStatistic(statisticType As IItemStatisticType) As Long
        Return If(WorldData.ItemStatistic.Read(Id, statisticType.Id), statisticType.DefaultValue.Value)
    End Function
    Public ReadOnly Property Maximum As Long? Implements IDurability.Maximum
        Get
            Return Item.FromId(WorldData, Id).ItemType.MaximumDurability
        End Get
    End Property
    Public Sub Reduce(amount As Long) Implements IDurability.Reduce
        If Maximum.HasValue Then
            ChangeStatistic(ItemStatisticType.FromId(WorldData, StatisticTypeDurability), amount)
        End If
    End Sub
    Private Sub ChangeStatistic(statisticType As IItemStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType) + delta)
    End Sub
    Private Sub SetStatistic(statisticType As IItemStatisticType, value As Long)
        WorldData.ItemStatistic.Write(Id, statisticType.Id, value)
    End Sub
    ReadOnly Property IsBroken As Boolean Implements IDurability.IsBroken
        Get
            Return Maximum.HasValue AndAlso Current.Value <= 0
        End Get
    End Property
End Class
