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
    Public Sub Purify() Implements IItem.Purify
        ItemType.Purify(Me)
    End Sub

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
    Public ReadOnly Property Usage As IUsage Implements IItem.Usage
        Get
            Return Game.Usage.FromId(WorldData, Id)
        End Get
    End Property
End Class
