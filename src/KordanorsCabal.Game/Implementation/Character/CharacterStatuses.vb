Public Class CharacterStatuses
    Inherits SubcharacterBase
    Implements ICharacterStatuses

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterStatuses
        Return If(character IsNot Nothing, New CharacterStatuses(worldData, character), Nothing)
    End Function
    Property Money As Long Implements ICharacterStatuses.Money
        Get
            Return If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType14)), 0L)
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType14), value)
        End Set
    End Property
    Sub DoImmobilization(delta As Long) Implements ICharacterStatuses.DoImmobilization
        Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType23), delta)
    End Sub
    ReadOnly Property IsUndead As Boolean Implements ICharacterStatuses.IsUndead
        Get
            Return character.CharacterType.IsUndead
        End Get
    End Property
    Property Drunkenness As Long Implements ICharacterStatuses.Drunkenness
        Get
            Return Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType18)).Value
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType18), value)
        End Set
    End Property
    Property Chafing As Long Implements ICharacterStatuses.Chafing
        Get
            Return Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType22)).Value
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType22), value)
        End Set
    End Property
    Property Highness As Long Implements ICharacterStatuses.Highness
        Get
            Return If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType19)), 0)
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType19), value)
        End Set
    End Property
    Property Hunger As Long Implements ICharacterStatuses.Hunger
        Get
            Return Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType20)).Value
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType20), value)
        End Set
    End Property
    Public Property FoodPoisoning As Long Implements ICharacterStatuses.FoodPoisoning
        Get
            Return Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType21)).Value
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType21), value)
        End Set
    End Property
End Class
