Public Class Weapon
    Inherits BaseThingie
    Implements IWeapon
    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Public Shared Function FromId(worldData As IWorldData, id As Long?) As IWeapon
        Return If(id.HasValue, New Weapon(worldData, id.Value), Nothing)
    End Function
    ReadOnly Property IsWeapon() As Boolean Implements IWeapon.IsWeapon
        Get
            Return AttackDice > 0
        End Get
    End Property
    ReadOnly Property AttackDice As Long Implements IWeapon.AttackDice
        Get
            Return If(WorldData.ItemStatistic.Read(Id, StatisticTypeAttackDice), 0)
        End Get
    End Property
    ReadOnly Property MaximumDamage As Long? Implements IWeapon.MaximumDamage
        Get
            Return WorldData.ItemStatistic.Read(Id, StatisticTypeMaximumDamage)
        End Get
    End Property
End Class
