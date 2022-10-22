Public Class CharacterMana
    Inherits SubcharacterBase
    Implements ICharacterMana

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterMana
        Return If(character IsNot Nothing, New CharacterMana(worldData, character), Nothing)
    End Function
    Property CurrentMana As Long Implements ICharacterMana.CurrentMana
        Get
            Dim maximumMana = Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType8))
            Dim fatigue = Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType15))
            Return Math.Max(0, If(maximumMana, 0L) - If(fatigue, 0L))
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType15), Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType8)).Value - value)
        End Set
    End Property
    Sub DoFatigue(fatigue As Long) Implements ICharacterMana.DoFatigue
        Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType15), fatigue)
    End Sub
    Public ReadOnly Property MaximumMana As Long Implements ICharacterMana.MaximumMana
        Get
            Return Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, CharacterStatisticType8)).Value
        End Get
    End Property
End Class
