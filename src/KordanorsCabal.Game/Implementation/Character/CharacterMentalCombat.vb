Public Class CharacterMentalCombat
    Inherits SubcharacterBase
    Implements ICharacterMentalCombat
    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character)
    End Sub
    Public Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterMentalCombat
        Return If(character IsNot Nothing, New CharacterMentalCombat(worldData, character), Nothing)
    End Function
    Function RollInfluence() As Long Implements ICharacterMentalCombat.RollInfluence
        Return RollDice(If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeInfluence)), 0) + NegativeInfluence())
    End Function
    Function RollWillpower() As Long Implements ICharacterMentalCombat.RollWillpower
        Return RollDice(If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWillpower)), 0) + NegativeInfluence())
    End Function
    ReadOnly Property IsDemoralized() As Boolean Implements ICharacterMentalCombat.IsDemoralized
        Get
            If Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWillpower)).HasValue Then
                Return CurrentMP <= 0
            End If
            Return False
        End Get
    End Property
    Sub AddStress(delta As Long) Implements ICharacterMentalCombat.AddStress
        Character.Statistics.ChangeStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeStress), delta)
    End Sub
    ReadOnly Property CanIntimidate As Boolean Implements ICharacterMentalCombat.CanIntimidate
        Get
            If Not Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeWillpower)).HasValue Then
                Return False
            End If
            Return character.Movement.Location.Factions.AlliesOf(character).Count <= character.Movement.Location.Factions.EnemiesOf(character).Count
        End Get
    End Property
    Property CurrentMP As Long Implements ICharacterMentalCombat.CurrentMP
        Get
            Return Math.Max(0, Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeMP)).Value - Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeStress)).Value)
        End Get
        Set(value As Long)
            Character.Statistics.SetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeStress), Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeMP)).Value - value)
        End Set
    End Property
    ReadOnly Property CanDoIntimidation() As Boolean Implements ICharacterMentalCombat.CanDoIntimidation
        Get
            If If(Character.Statistics.GetStatistic(CharacterStatisticType.FromId(WorldData, StatisticTypeInfluence)), 0) <= 0 Then
                Return False
            End If
            Dim enemy = character.Movement.Location.Factions.EnemiesOf(character).FirstOrDefault
            If enemy Is Nothing Then
                Return False
            End If
            Return enemy.MentalCombat.CanIntimidate
        End Get
    End Property
    Public Sub DoIntimidation() Implements ICharacterMentalCombat.DoIntimidation
        If CanDoIntimidation Then
            Dim lines As New List(Of String)
            Dim enemy = character.Movement.Location.Factions.EnemiesOf(character).First
            Dim influenceRoll = RollInfluence()
            lines.Add($"You roll {influenceRoll} influence.")
            Dim willpowerRoll = enemy.MentalCombat.RollWillpower()
            lines.Add($"{enemy.CharacterType.Name} rolls {willpowerRoll} willpower.")
            If influenceRoll > willpowerRoll Then
                enemy.MentalCombat.AddStress(1)
                lines.Add($"{enemy.CharacterType.Name} loses 1 MP!")
                If enemy.MentalCombat.IsDemoralized Then
                    lines.Add($"{enemy.CharacterType.Name} runs away!")
                    enemy.Destroy()
                End If
            Else
                lines.Add($"{enemy.CharacterType.Name} is not intimidated.")
            End If
            character.EnqueueMessage(Nothing, lines.ToArray)
            character.PhysicalCombat.DoCounterAttacks()
            Return
        End If
        character.EnqueueMessage(Nothing, "You cannot intimidate at this time!")
    End Sub
End Class
