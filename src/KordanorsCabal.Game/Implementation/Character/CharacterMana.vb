Public Class CharacterMana
    Inherits BaseThingie
    Implements ICharacterMana
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterMana
        Return If(character IsNot Nothing, New CharacterMana(worldData, character), Nothing)
    End Function
    Property CurrentMana As Long Implements ICharacterMana.CurrentMana
        Get
            Dim maximumMana = character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Mana))
            Dim fatigue = character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Fatigue))
            Return Math.Max(0, If(maximumMana, 0L) - If(fatigue, 0L))
        End Get
        Set(value As Long)
            character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Fatigue), character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Mana)).Value - value)
        End Set
    End Property
    Sub DoFatigue(fatigue As Long) Implements ICharacterMana.DoFatigue
        character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Fatigue), fatigue)
    End Sub
    Public ReadOnly Property MaximumMana As Long Implements ICharacterMana.MaximumMana
        Get
            Return character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Mana)).Value
        End Get
    End Property
End Class
