Public Class CharacterHealth
    Inherits BaseThingie
    Implements ICharacterHealth

    Public Sub New(worldData As IWorldData, id As Long)
        MyBase.New(worldData, id)
    End Sub
    Shared Function FromId(worldData As IWorldData, id As Long?) As ICharacterHealth
        Return If(id.HasValue, New CharacterHealth(worldData, id.Value), Nothing)
    End Function
    Property CurrentHP As Long Implements ICharacterHealth.CurrentHP
        Get
            Return Math.Max(0, MaximumHP - If(Character.FromId(WorldData, Id).GetStatistic(CharacterStatisticType.FromId(WorldData, 12L)), 0))
        End Get
        Set(value As Long)
            Character.FromId(WorldData, Id).SetStatistic(CharacterStatisticType.FromId(WorldData, 12L), Character.FromId(WorldData, Id).GetStatistic(CharacterStatisticType.FromId(WorldData, 6L)).Value - value)
        End Set
    End Property
    ReadOnly Property MaximumHP As Long Implements ICharacterHealth.MaximumHP
        Get
            Return If(Character.FromId(WorldData, Id).GetStatistic(CharacterStatisticType.FromId(WorldData, 6L)), 0)
        End Get
    End Property
    ReadOnly Property IsDead As Boolean Implements ICharacterHealth.IsDead
        Get
            Return CurrentHP <= 0
        End Get
    End Property
    ReadOnly Property NeedsHealing As Boolean Implements ICharacterHealth.NeedsHealing
        Get
            Return Character.FromId(WorldData, Id).GetStatistic(CharacterStatisticType.FromId(WorldData, 12L)).Value > 0
        End Get
    End Property
    Public Sub Heal() Implements ICharacterHealth.Heal
        Character.FromId(WorldData, Id).SetStatistic(CharacterStatisticType.FromId(WorldData, 12L), 0)
    End Sub
End Class
