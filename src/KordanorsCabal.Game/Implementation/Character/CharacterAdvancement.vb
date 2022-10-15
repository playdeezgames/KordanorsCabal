Public Class CharacterAdvancement
    Inherits BaseThingie
    Implements ICharacterAdvancement
    Private character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub

    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterAdvancement
        Return If(character IsNot Nothing, New CharacterAdvancement(worldData, character), Nothing)
    End Function
    Function AddXP(xp As Long) As Boolean Implements ICharacterAdvancement.AddXP
        character.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.XP), xp)
        Dim xpGoal = character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.XPGoal))
        If Not xpGoal.HasValue Then
            Return False
        End If
        If character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.XP)).Value >= xpGoal Then
            character.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.XP), -xpGoal.Value)
            character.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.XPGoal), xpGoal.Value)
            character.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Unassigned), 1)
            character.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Wounds), 0)
            character.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Stress), 0)
            character.SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Fatigue), 0)
            Return True
        End If
        Return False
    End Function
    ReadOnly Property IsFullyAssigned As Boolean Implements ICharacterAdvancement.IsFullyAssigned
        Get
            Return If(character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Unassigned)), 0) = 0
        End Get
    End Property
    Public Sub AssignPoint(statisticType As ICharacterStatisticType) Implements ICharacterAdvancement.AssignPoint
        If Not IsFullyAssigned Then
            character.ChangeStatistic(statisticType, 1)
            character.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Unassigned), -1)
        End If
    End Sub
End Class
