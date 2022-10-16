Public Class CharacterStatuses
    Inherits BaseThingie
    Implements ICharacterStatuses
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterStatuses
        Return If(character IsNot Nothing, New CharacterStatuses(worldData, character), Nothing)
    End Function
    Property Money As Long Implements ICharacterStatuses.Money
        Get
            Return If(character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Money)), 0L)
        End Get
        Set(value As Long)
            character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Money), value)
        End Set
    End Property
    Sub DoImmobilization(delta As Long) Implements ICharacterStatuses.DoImmobilization
        character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Immobilization), delta)
    End Sub
    ReadOnly Property IsUndead As Boolean Implements ICharacterStatuses.IsUndead
        Get
            Return character.CharacterType.IsUndead
        End Get
    End Property
    Property Drunkenness As Long Implements ICharacterStatuses.Drunkenness
        Get
            Return character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Drunkenness)).Value
        End Get
        Set(value As Long)
            character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Drunkenness), value)
        End Set
    End Property
    Property Chafing As Long Implements ICharacterStatuses.Chafing
        Get
            Return character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Chafing)).Value
        End Get
        Set(value As Long)
            character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Chafing), value)
        End Set
    End Property
    Property Highness As Long Implements ICharacterStatuses.Highness
        Get
            Return If(character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Highness)), 0)
        End Get
        Set(value As Long)
            character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Highness), value)
        End Set
    End Property
    Property Hunger As Long Implements ICharacterStatuses.Hunger
        Get
            Return character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Hunger)).Value
        End Get
        Set(value As Long)
            character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Hunger), value)
        End Set
    End Property
    Public Property FoodPoisoning As Long Implements ICharacterStatuses.FoodPoisoning
        Get
            Return character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.FoodPoisoning)).Value
        End Get
        Set(value As Long)
            character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.FoodPoisoning), value)
        End Set
    End Property
End Class
