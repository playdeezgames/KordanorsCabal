Public Class Item
    Inherits BaseThingie
    Implements IItem
    Sub New(worldData As IWorldData, itemId As Long)
        MyBase.New(worldData, itemId)
    End Sub
    Shared Function FromId(worldData As IWorldData, itemId As Long?) As IItem
        Return If(itemId.HasValue, New Item(worldData, itemId.Value), Nothing)
    End Function
    Shared Function Create(worldData As IWorldData, itemType As Long) As IItem
        Return FromId(worldData, worldData.Item.Create(itemType))
    End Function
    Public ReadOnly Property ItemType As IItemType Implements IItem.ItemType
        Get
            Return Game.ItemType.FromId(WorldData, WorldData.Item.ReadItemType(Id))
        End Get
    End Property
    Public ReadOnly Property Name As String Implements IItem.Name
        Get
            Return ItemType.Name
        End Get
    End Property
    ReadOnly Property CanUse(character As ICharacter) As Boolean Implements IItem.CanUse
        Get
            Return ItemType.CanUse(character)
        End Get
    End Property
    ReadOnly Property CanEquip As Boolean Implements IItem.CanEquip
        Get
            Return ItemType.EquipSlots.Any
        End Get
    End Property

    Public Function RepairCost(shoppeType As ShoppeType) As Long Implements IItem.RepairCost
        Dim fullRepairPrice = shoppeType.RepairPrice(ItemType)
        Dim wear = GetStatistic(ItemStatisticType.Wear)
        Dim maximum = Durability.Maximum.Value
        Dim remainder = If(wear * fullRepairPrice Mod maximum > 0, 1, 0)
        Return wear * If(fullRepairPrice, 0) \ maximum + remainder
    End Function

    Public Sub Purify() Implements IItem.Purify
        ItemType.Purify.Invoke(Me)
    End Sub

    Public Sub Destroy() Implements IItem.Destroy
        WorldData.Item.Clear(Id)
    End Sub
    Public ReadOnly Property Encumbrance As Long Implements IItem.Encumbrance
        Get
            Return ItemType.Encumbrance
        End Get
    End Property
    Friend Sub Use(character As ICharacter) Implements IItem.Use
        ItemType.Use.Invoke(WorldData, character)
    End Sub
    ReadOnly Property EquipSlots() As IEnumerable(Of IEquipSlot) Implements IItem.EquipSlots
        Get
            Return ItemType.EquipSlots
        End Get
    End Property
    Private Sub ChangeStatistic(statisticType As ItemStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType) + delta)
    End Sub

    Private Function GetStatistic(statisticType As ItemStatisticType) As Long
        Return If(WorldData.ItemStatistic.Read(Id, statisticType), statisticType.DefaultValue)
    End Function
    Private Sub SetStatistic(statisticType As ItemStatisticType, value As Long)
        WorldData.ItemStatistic.Write(Id, statisticType, value)
    End Sub
    ReadOnly Property DefendDice As Long Implements IItem.DefendDice
        Get
            Return ItemType.DefendDice
        End Get
    End Property
    ReadOnly Property IsArmor() As Boolean Implements IItem.IsArmor
        Get
            Return ItemType.IsArmor
        End Get
    End Property
    ReadOnly Property IsConsumed As Boolean Implements IItem.IsConsumed
        Get
            Return ItemType.IsConsumed
        End Get
    End Property

    Public ReadOnly Property Weapon As IWeapon Implements IItem.Weapon
        Get
            Return Game.Weapon.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Durability As IDurability Implements IItem.Durability
        Get
            Return Game.Durability.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Repair As IRepair Implements IItem.Repair
        Get
            Return Game.Repair.FromId(WorldData, Id)
        End Get
    End Property

    Function EquippedBuff(statisticType As ICharacterStatisticType) As Long? Implements IItem.EquippedBuff
        Return ItemType.EquippedBuff(statisticType)
    End Function
End Class
