Public Class CharacterAdvancement
    Inherits SubcharacterBase
    Implements ICharacterAdvancement

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterAdvancement
        Return If(character IsNot Nothing, New CharacterAdvancement(worldData, character), Nothing)
    End Function
    Function AddXP(xp As Long) As Boolean Implements ICharacterAdvancement.AddXP
        Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeXP), xp)
        Dim xpGoal = Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeXPGoal))
        If Not xpGoal.HasValue Then
            Return False
        End If
        If Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeXP)).Value >= xpGoal Then
            Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeXP), -xpGoal.Value)
            Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeXPGoal), xpGoal.Value)
            Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeUnassigned), 1)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWounds), 0)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeStress), 0)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeFatigue), 0)
            Return True
        End If
        Return False
    End Function
    ReadOnly Property IsFullyAssigned As Boolean Implements ICharacterAdvancement.IsFullyAssigned
        Get
            Return If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeUnassigned)), 0) = 0
        End Get
    End Property
    Public Sub AssignPoint(statisticType As ICharacterStatisticType) Implements ICharacterAdvancement.AssignPoint
        If Not IsFullyAssigned Then
            character.Statistics.ChangeStatistic(statisticType, 1)
            Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeUnassigned), -1)
        End If
    End Sub
End Class
