Public Class Item
    ReadOnly Property Id As Long
    Sub New(itemId As Long)
        Id = itemId
    End Sub
    Shared Function FromId(itemId As Long?) As Item
        Return If(itemId.HasValue, New Item(itemId.Value), Nothing)
    End Function
    Shared Function Create(itemType As ItemType) As Item
        Return FromId(StaticWorldData.World.Item.Create(itemType))
    End Function
    Public ReadOnly Property ItemType As ItemType
        Get
            Return CType(StaticWorldData.World.Item.ReadItemType(Id).Value, ItemType)
        End Get
    End Property
    Public ReadOnly Property Name As String
        Get
            Return ItemType.Name
        End Get
    End Property
    ReadOnly Property CanUse(character As Character) As Boolean
        Get
            Return ItemType.CanUse(character)
        End Get
    End Property
    ReadOnly Property CanEquip As Boolean
        Get
            Return ItemType.EquipSlots.Any
        End Get
    End Property

    Public Function RepairCost(shoppeType As ShoppeType) As Long
        Dim fullRepairPrice = shoppeType.Repairs(ItemType)
        Dim wear = GetStatistic(ItemStatisticType.Wear)
        Dim maximum = MaximumDurability.Value
        Dim remainder = If(wear * fullRepairPrice Mod maximum > 0, 1, 0)
        Return wear * fullRepairPrice \ maximum + remainder
    End Function

    Friend Sub Purify()
        ItemType.Purify(Me)
    End Sub

    Public Sub Destroy()
        StaticWorldData.World.Item.Clear(Id)
    End Sub
    Public Function Encumbrance() As Long
        Return ItemType.Encumbrance
    End Function
    Friend Sub Use(character As Character)
        ItemType.Use(character)
    End Sub
    Friend ReadOnly Property EquipSlots() As IEnumerable(Of EquipSlot)
        Get
            Return ItemType.EquipSlots
        End Get
    End Property
    Friend ReadOnly Property MaximumDamage As Long?
        Get
            Return ItemType.MaximumDamage
        End Get
    End Property
    Friend ReadOnly Property AttackDice As Long
        Get
            Return ItemType.AttackDice
        End Get
    End Property
    Public ReadOnly Property MaximumDurability As Long?
        Get
            Return ItemType.MaximumDurability
        End Get
    End Property
    Friend Sub ReduceDurability(amount As Long)
        If MaximumDurability.HasValue Then
            ChangeStatistic(ItemStatisticType.Wear, amount)
        End If
    End Sub
    Private Sub ChangeStatistic(statisticType As ItemStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType) + delta)
    End Sub

    Public Sub Repair()
        SetStatistic(ItemStatisticType.Wear, 0)
    End Sub

    Private Function GetStatistic(statisticType As ItemStatisticType) As Long
        Return If(StaticWorldData.World.ItemStatistic.Read(Id, statisticType), statisticType.DefaultValue)
    End Function
    Private Sub SetStatistic(statisticType As ItemStatisticType, value As Long)
        StaticWorldData.World.ItemStatistic.Write(Id, statisticType, value)
    End Sub

    Public ReadOnly Property NeedsRepair As Boolean
        Get
            Return MaximumDurability.HasValue AndAlso Durability.Value < MaximumDurability.Value
        End Get
    End Property

    ReadOnly Property IsBroken As Boolean
        Get
            Return MaximumDurability.HasValue AndAlso Durability.Value <= 0
        End Get
    End Property
    ReadOnly Property Durability As Long?
        Get
            If MaximumDurability.HasValue Then
                Return MaximumDurability.Value - GetStatistic(ItemStatisticType.Wear)
            End If
            Return Nothing
        End Get
    End Property
    ReadOnly Property DefendDice As Long
        Get
            Return ItemType.DefendDice
        End Get
    End Property
    ReadOnly Property IsWeapon() As Boolean
        Get
            Return ItemType.IsWeapon
        End Get
    End Property
    ReadOnly Property IsArmor() As Boolean
        Get
            Return ItemType.IsArmor
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean
        Get
            Return ItemType.IsConsumed
        End Get
    End Property

    Friend Function EquippedBuff(statisticType As CharacterStatisticType) As Long?
        Return ItemType.EquippedBuff(statisticType)
    End Function
End Class
