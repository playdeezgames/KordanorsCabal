Public Class ItemTypeCombat
    Inherits BaseThingie
    Implements IItemTypeCombat

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As IItemTypeCombat
        Return If(id.HasValue, New ItemTypeCombat(worldData, id.Value), Nothing)
    End Function
    Public ReadOnly Property IsWeapon As Boolean Implements IItemTypeCombat.IsWeapon
        Get
            Return AttackDice > 0
        End Get
    End Property
    Public ReadOnly Property IsArmor As Boolean Implements IItemTypeCombat.IsArmor
        Get
            Return DefendDice > 0
        End Get
    End Property
    ReadOnly Property AttackDice As Long Implements IItemTypeCombat.AttackDice
        Get
            Return If(ItemType.ItemTypeStatistic(WorldData, Id, ItemTypeStatisticType.FromId(WorldData, StatisticTypeAttackDice)), 0)
        End Get
    End Property
    ReadOnly Property MaximumDamage As Long? Implements IItemTypeCombat.MaximumDamage
        Get
            Return ItemType.ItemTypeStatistic(WorldData, Id, ItemTypeStatisticType.FromId(WorldData, StatisticTypeMaximumDamage))
        End Get
    End Property
    ReadOnly Property DefendDice As Long Implements IItemTypeCombat.DefendDice
        Get
            Return If(ItemType.ItemTypeStatistic(WorldData, Id, ItemTypeStatisticType.FromId(WorldData, StatisticTypeDefendDice)), 0)
        End Get
    End Property
End Class
