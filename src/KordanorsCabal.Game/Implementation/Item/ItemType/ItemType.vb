Public Class ItemType
    Inherits BaseItemType
    Implements IItemType
    Public Shared Function FromId(worlddata As IWorldData, id As Long?) As IItemType
        Return If(id.HasValue, New ItemType(worlddata, id.Value), Nothing)
    End Function
    ReadOnly Property Name As String Implements IItemType.Name
        Get
            Return WorldData.ItemType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property MaximumDurability As Long? Implements IItemType.MaximumDurability
        Get
            Return Statistic(Id, StatisticType.FromId(WorldData, StatisticTypeMaximumDurability))
        End Get
    End Property
    ReadOnly Property Offer As Long Implements IItemType.Offer
        Get
            Return If(Statistic(Id, StatisticType.FromId(WorldData, StatisticTypeOffer)), 0)
        End Get
    End Property
    ReadOnly Property Price As Long Implements IItemType.Price
        Get
            Return If(Statistic(Id, StatisticType.FromId(WorldData, StatisticTypePrice)), 0)
        End Get
    End Property
    ReadOnly Property RepairPrice As Long Implements IItemType.RepairPrice
        Get
            Return If(Statistic(Id, StatisticType.FromId(WorldData, StatisticTypeRepairPrice)), 0)
        End Get
    End Property
    Private Const OfferTransactionTypeId = 1L
    Private Const PriceTransactionTypeId = 2L
    Private Const RepairTransactionTypeId = 3L
    Private ReadOnly Property boughtAt As IEnumerable(Of Long)
        Get
            Return WorldData.ItemTypeShopType.
                ReadForTransactionType(Id, OfferTransactionTypeId)
        End Get
    End Property
    Private ReadOnly Property soldAt As IEnumerable(Of Long)
        Get
            Return WorldData.ItemTypeShopType.
                ReadForTransactionType(Id, PriceTransactionTypeId)
        End Get
    End Property
    Private ReadOnly Property repairedAt As IEnumerable(Of Long)
        Get
            Return WorldData.ItemTypeShopType.
                ReadForTransactionType(Id, RepairTransactionTypeId)
        End Get
    End Property
    Public Const PurifyEventId = 1L
    Public Const CanUseEventId = 2L
    Public Const UseEventId = 3L
    Public Const DecayEventId = 4L
    Public Const AddToInventoryEventId = 5L
    Private ReadOnly Property UseActionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, UseEventId)
        End Get
    End Property
    Private ReadOnly Property PurifyActionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, PurifyEventId)
        End Get
    End Property
    Sub Purify(item As IItem) Implements IItemType.Purify
        Dim eventName = PurifyActionName
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, item.Id)
        End If
    End Sub
    ReadOnly Property HasOffer(shoppeType As IShoppeType) As Boolean Implements IItemType.HasOffer
        Get
            Return boughtAt.Contains(shoppeType.Id)
        End Get
    End Property
    ReadOnly Property HasPrice(shoppeType As IShoppeType) As Boolean Implements IItemType.HasPrice
        Get
            Return soldAt.Contains(shoppeType.Id)
        End Get
    End Property
    ReadOnly Property CanRepair(shoppeType As IShoppeType) As Boolean Implements IItemType.CanRepair
        Get
            Return repairedAt.Contains(shoppeType.Id)
        End Get
    End Property
    Sub New(
           worldData As IWorldData,
           itemTypeId As Long)
        MyBase.New(worldData, itemTypeId)
    End Sub
    Private ReadOnly Property DecayActionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, DecayEventId)
        End Get
    End Property

    Public ReadOnly Property Spawn As IItemTypeSpawn Implements IItemType.Spawn
        Get
            Return ItemTypeSpawn.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Equip As IItemTypeEquip Implements IItemType.Equip
        Get
            Return ItemTypeEquip.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Combat As IItemTypeCombat Implements IItemType.Combat
        Get
            Return ItemTypeCombat.FromId(WorldData, Id)
        End Get
    End Property
    Public Sub Decay(item As IItem) Implements IItemType.Decay
        Dim result As Action(Of IWorldData, IItem) = Nothing
        Dim eventName = DecayActionName
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, item.Id)
        End If
    End Sub
End Class
