Public Class Item
    Inherits BaseThingie
    Implements IItem
    Sub New(worldData As IWorldData, itemId As Long)
        MyBase.New(worldData, itemId)
    End Sub
    Shared Function FromId(worldData As IWorldData, itemId As Long?) As IItem
        Return If(itemId.HasValue, New Item(worldData, itemId.Value), Nothing)
    End Function
    Shared Function Create(worldData As IWorldData, itemType As OldItemType) As IItem
        Return FromId(worldData, worldData.Item.Create(itemType))
    End Function
    Public ReadOnly Property ItemType As OldItemType Implements IItem.ItemType
        Get
            Return CType(WorldData.Item.ReadItemType(Id).Value, OldItemType)
        End Get
    End Property
    Public ReadOnly Property Name As String Implements IItem.Name
        Get
            Return ItemType.ToNew(WorldData).Name
        End Get
    End Property
    ReadOnly Property CanUse(character As ICharacter) As Boolean Implements IItem.CanUse
        Get
            Return ItemType.ToNew(WorldData).CanUse(character)
        End Get
    End Property
    ReadOnly Property CanEquip As Boolean Implements IItem.CanEquip
        Get
            Return ItemType.ToNew(WorldData).EquipSlots.Any
        End Get
    End Property

    Public Function RepairCost(shoppeType As ShoppeType) As Long Implements IItem.RepairCost
        Dim fullRepairPrice = shoppeType.Repairs(ItemType)
        Dim wear = GetStatistic(ItemStatisticType.Wear)
        Dim maximum = MaximumDurability.Value
        Dim remainder = If(wear * fullRepairPrice Mod maximum > 0, 1, 0)
        Return wear * fullRepairPrice \ maximum + remainder
    End Function

    Public Sub Purify() Implements IItem.Purify
        ItemType.Purify(Me)
    End Sub

    Public Sub Destroy() Implements IItem.Destroy
        WorldData.Item.Clear(Id)
    End Sub
    Public Function Encumbrance() As Long Implements IItem.Encumbrance
        Return ItemType.ToNew(WorldData).Encumbrance
    End Function
    Friend Sub Use(character As ICharacter) Implements IItem.Use
        ItemType.ToNew(WorldData).Use.Invoke(character)
    End Sub
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot) Implements IItem.EquipSlots
        Get
            Return ItemType.ToNew(WorldData).EquipSlots
        End Get
    End Property
    ReadOnly Property MaximumDamage As Long? Implements IItem.MaximumDamage
        Get
            Return ItemType.ToNew(WorldData).MaximumDamage
        End Get
    End Property
    ReadOnly Property AttackDice As Long Implements IItem.AttackDice
        Get
            Return ItemType.ToNew(WorldData).AttackDice
        End Get
    End Property
    Public ReadOnly Property MaximumDurability As Long? Implements IItem.MaximumDurability
        Get
            Return ItemType.ToNew(WorldData).MaximumDurability
        End Get
    End Property
    Public Sub ReduceDurability(amount As Long) Implements IItem.ReduceDurability
        If MaximumDurability.HasValue Then
            ChangeStatistic(ItemStatisticType.Wear, amount)
        End If
    End Sub
    Private Sub ChangeStatistic(statisticType As ItemStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType) + delta)
    End Sub

    Public Sub Repair() Implements IItem.Repair
        SetStatistic(ItemStatisticType.Wear, 0)
    End Sub

    Private Function GetStatistic(statisticType As ItemStatisticType) As Long
        Return If(WorldData.ItemStatistic.Read(Id, statisticType), statisticType.DefaultValue)
    End Function
    Private Sub SetStatistic(statisticType As ItemStatisticType, value As Long)
        WorldData.ItemStatistic.Write(Id, statisticType, value)
    End Sub

    Public ReadOnly Property NeedsRepair As Boolean Implements IItem.NeedsRepair
        Get
            Return MaximumDurability.HasValue AndAlso Durability.Value < MaximumDurability.Value
        End Get
    End Property

    ReadOnly Property IsBroken As Boolean Implements IItem.IsBroken
        Get
            Return MaximumDurability.HasValue AndAlso Durability.Value <= 0
        End Get
    End Property
    ReadOnly Property Durability As Long? Implements IItem.Durability
        Get
            If MaximumDurability.HasValue Then
                Return MaximumDurability.Value - GetStatistic(ItemStatisticType.Wear)
            End If
            Return Nothing
        End Get
    End Property
    ReadOnly Property DefendDice As Long Implements IItem.DefendDice
        Get
            Return ItemType.ToNew(WorldData).DefendDice
        End Get
    End Property
    ReadOnly Property IsWeapon() As Boolean Implements IItem.IsWeapon
        Get
            Return ItemType.IsWeapon
        End Get
    End Property
    ReadOnly Property IsArmor() As Boolean Implements IItem.IsArmor
        Get
            Return ItemType.IsArmor
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean Implements IItem.IsConsumed
        Get
            Return ItemType.ToNew(WorldData).IsConsumed
        End Get
    End Property

    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long? Implements IItem.EquippedBuff
        Return ItemType.ToNew(WorldData).EquippedBuff(statisticType)
    End Function
End Class
