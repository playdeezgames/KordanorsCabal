Public Class ItemType
    Inherits BaseThingie
    Implements IItemType
    Public Shared Function FromId(worlddata As IWorldData, id As Long?) As IItemType
        Return If(id.HasValue, New ItemType(worlddata, id.Value), Nothing)
    End Function
    ReadOnly Property Name As String Implements IItemType.Name
        Get
            Return WorldData.ItemType.ReadName(Id)
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean Implements IItemType.IsConsumed
        Get
            Return If(WorldData.ItemType.ReadIsConsumed(Id), 0) > 0
        End Get
    End Property
    Private ReadOnly Property ItemTypeStatistic(itemTypeStatisticType As IItemTypeStatisticType) As Long?
        Get
            Return WorldData.ItemTypeStatistic.Read(Id, itemTypeStatisticType.Id)
        End Get
    End Property
    ReadOnly Property Encumbrance As Long Implements IItemType.Encumbrance
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 1)), 0)
        End Get
    End Property
    ReadOnly Property AttackDice As Long Implements IItemType.AttackDice
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 2)), 0)
        End Get
    End Property
    ReadOnly Property MaximumDamage As Long? Implements IItemType.MaximumDamage
        Get
            Return ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 3))
        End Get
    End Property
    ReadOnly Property DefendDice As Long Implements IItemType.DefendDice
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 4)), 0)
        End Get
    End Property
    ReadOnly Property MaximumDurability As Long? Implements IItemType.MaximumDurability
        Get
            Return ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 5))
        End Get
    End Property
    ReadOnly Property Offer As Long Implements IItemType.Offer
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 6)), 0)
        End Get
    End Property
    ReadOnly Property Price As Long Implements IItemType.Price
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 7)), 0)
        End Get
    End Property
    ReadOnly Property RepairPrice As Long Implements IItemType.RepairPrice
        Get
            Return If(ItemTypeStatistic(ItemTypeStatisticType.FromId(WorldData, 8)), 0)
        End Get
    End Property
    ReadOnly Property EquipSlots As IEnumerable(Of IEquipSlot) Implements IItemType.EquipSlots
        Get
            Return WorldData.ItemTypeEquipSlot.ReadForItemType(Id).Select(Function(x) EquipSlot.FromId(WorldData, x))
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
    Const PurifyEventId = 1L
    Const CanUseEventId = 2L
    Const UseEventId = 3L
    Const DecayEventId = 4L
    Private ReadOnly Property UseActionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, UseEventId)
        End Get
    End Property
    Private ReadOnly Property CanUseFunctionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, CanUseEventId)
        End Get
    End Property
    Private ReadOnly Property PurifyActionName As String
        Get
            Return WorldData.ItemTypeEvent.Read(Id, PurifyEventId)
        End Get
    End Property
    Sub Purify(item As IItem) Implements IItemType.Purify
        Dim result As Action(Of IWorldData, IItem) = Nothing
        Dim eventName = PurifyActionName
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, item.Id)
        End If
    End Sub
    Sub Use(character As ICharacter) Implements IItemType.Use
        Dim eventName = UseActionName
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, character.Id)
        End If
    End Sub

    Function CanUse(Character As ICharacter) As Boolean Implements IItemType.CanUse
        Dim eventName = CanUseFunctionName
        If eventName IsNot Nothing Then
            Return WorldData.Events.Test(WorldData, eventName, Character.Id)
        End If
        Return False
    End Function

    Public ReadOnly Property IsWeapon As Boolean Implements IItemType.IsWeapon
        Get
            Return AttackDice > 0
        End Get
    End Property

    Public ReadOnly Property IsArmor As Boolean Implements IItemType.IsArmor
        Get
            Return DefendDice > 0
        End Get
    End Property
    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long? Implements IItemType.EquippedBuff
        Return WorldData.ItemTypeCharacterStatisticBuff.Read(Id, statisticType.Id)
    End Function
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

    Public Sub Decay(item As IItem) Implements IItemType.Decay
        Dim result As Action(Of IWorldData, IItem) = Nothing
        Dim eventName = DecayActionName
        If eventName IsNot Nothing Then
            WorldData.Events.Perform(WorldData, eventName, item.Id)
        End If
    End Sub
End Class
