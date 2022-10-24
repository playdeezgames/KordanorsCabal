Public Class CharacterHealth
    Inherits SubcharacterBase
    Implements ICharacterHealth

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub
    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterHealth
        Return If(character IsNot Nothing, New CharacterHealth(worldData, character), Nothing)
    End Function
    Property Current As Long Implements ICharacterHealth.Current
        Get
            Return Math.Max(0, Maximum - If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWounds)), 0))
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWounds), Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeHP)).Value - value)
        End Set
    End Property
    ReadOnly Property Maximum As Long Implements ICharacterHealth.Maximum
        Get
            Return If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeHP)), 0)
        End Get
    End Property
    ReadOnly Property IsDead As Boolean Implements ICharacterHealth.IsDead
        Get
            Return Current <= 0
        End Get
    End Property
    ReadOnly Property NeedsHealing As Boolean Implements ICharacterHealth.NeedsHealing
        Get
            Return Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWounds)).Value > 0
        End Get
    End Property
    Public Sub Heal() Implements ICharacterHealth.Heal
        Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWounds), 0)
    End Sub
End Class
