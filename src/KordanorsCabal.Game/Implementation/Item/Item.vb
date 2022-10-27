Public Class Item
    Inherits BaseThingie
    Implements IItem
    Sub New(worldData As IWorldData, itemId As Long)
        MyBase.New(worldData, itemId)
    End Sub
    Shared Function FromId(worldData As IWorldData, itemId As Long?) As IItem
        Return If(itemId.HasValue, New Item(worldData, itemId.Value), Nothing)
    End Function
    Shared Function Create(worldData As IWorldData, itemType As IItemType) As IItem
        Dim itemId = worldData.Item.Create(itemType.Id)
        Dim item = FromId(worldData, itemId)
        item.ItemType = itemType
        Return item
    End Function
    Public Property ItemType As IItemType Implements IItem.ItemType
        Get
            Return Game.ItemType.FromId(WorldData, WorldData.Item.ReadItemType(Id))
        End Get
        Set(value As IItemType)
            WorldData.ItemStatistic.ClearForItem(Id)
            WorldData.ItemEvent.ClearForItem(Id)
            WorldData.Item.WriteName(Id, value.Name)
            Dim statisticValues = WorldData.ItemTypeStatistic.ReadAll(value.Id)
            For Each statisticValue In statisticValues
                WorldData.ItemStatistic.Write(Id, statisticValue.Item1, statisticValue.Item2)
            Next
            Dim eventEntries As List(Of Tuple(Of Long, String)) = WorldData.ItemTypeEvent.ReadAll(value.Id)
            For Each eventEntry In eventEntries
                WorldData.ItemEvent.Write(Id, eventEntry.Item1, eventEntry.Item2)
            Next
            'TODO: run the "on create" event for the item
        End Set
    End Property
    Public Property Name As String Implements IItem.Name
        Get
            Return If(WorldData.Item.ReadName(Id), ItemType.Name)
        End Get
        Set(value As String)
            WorldData.Item.WriteName(Id, value)
        End Set
    End Property
    Public Sub Destroy() Implements IItem.Destroy
        WorldData.Item.Clear(Id)
    End Sub

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
    Public ReadOnly Property Armor As IArmor Implements IItem.Armor
        Get
            Return Game.Armor.FromId(WorldData, Id)
        End Get
    End Property
    Public ReadOnly Property Equipment As IEquipment Implements IItem.Equipment
        Get
            Return Game.Equipment.FromId(WorldData, Id)
        End Get
    End Property
    Public ReadOnly Property Events As IItemEvents Implements IItem.Events
        Get
            Return Game.ItemEvents.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Inventory As IInventory Implements IItem.Inventory
        Get
            Return Game.Inventory.FromId(WorldData, WorldData.InventoryItem.ReadForItem(Id))
        End Get
    End Property

    Public Property Lore As ILore Implements IItem.Lore
        Get
            Return Game.Lore.FromId(WorldData, WorldData.ItemLore.ReadForItem(Id))
        End Get
        Set(value As ILore)
            If value Is Nothing Then
                WorldData.ItemLore.ClearForItem(Id)
            Else
                WorldData.ItemLore.Write(Id, value.Id)
            End If
        End Set
    End Property

    Public ReadOnly Property Encumbrance As Long Implements IItem.Encumbrance
        Get
            Return If(WorldData.ItemStatistic.Read(Id, StatisticTypeEncumbrance), 0L)
        End Get
    End Property
End Class
