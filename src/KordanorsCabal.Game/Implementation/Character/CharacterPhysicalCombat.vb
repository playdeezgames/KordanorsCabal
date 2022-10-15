﻿Public Class CharacterPhysicalCombat
    Inherits BaseThingie
    Implements ICharacterPhysicalCombat
    Private ReadOnly character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub
    Shared Function FromCharacter(worldData As IWorldData, character As ICharacter) As ICharacterPhysicalCombat
        Return If(character IsNot Nothing, New CharacterPhysicalCombat(worldData, character), Nothing)
    End Function
    ReadOnly Property CanFight As Boolean Implements ICharacterPhysicalCombat.CanFight
        Get
            If character.Movement.Location Is Nothing Then
                Return False
            End If
            Return character.Movement.Location.Factions.EnemiesOf(character).Any
        End Get
    End Property
    ReadOnly Property IsEnemy(character As ICharacter) As Boolean Implements ICharacterPhysicalCombat.IsEnemy
        Get
            Return Me.character.CharacterType.Combat.IsEnemy(character.CharacterType)
        End Get
    End Property
    Function RollDefend() As Long Implements ICharacterPhysicalCombat.RollDefend
        Dim maximumDefend = character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.BaseMaximumDefend)).Value
        Return Math.Min(RollDice(GetDefendDice() + NegativeInfluence()), maximumDefend)
    End Function
    Function RollAttack() As Long Implements ICharacterPhysicalCombat.RollAttack
        Return RollDice(GetAttackDice() + NegativeInfluence())
    End Function
    Private Function RollDice(dice As Long) As Long
        Dim result As Long = 0
        While dice > 0
            result += RNG.RollDice("1d6/6")
            dice -= 1
        End While
        Return result
    End Function
    Private Function NegativeInfluence() As Long
        Return If(character.Drunkenness > 0 OrElse character.Highness > 0 OrElse character.Chafing > 0, -1, 0)
    End Function
    ReadOnly Property PartingShot As String Implements ICharacterPhysicalCombat.PartingShot
        Get
            Return character.CharacterType.Combat.PartingShot
        End Get
    End Property
    Sub DoDamage(damage As Long) Implements ICharacterPhysicalCombat.DoDamage
        character.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Wounds), damage)
    End Sub
    ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
    Function DetermineDamage(value As Long) As Long Implements ICharacterPhysicalCombat.DetermineDamage
        Dim maximumDamage As Long? = Nothing
        For Each item In EquippedItems
            Dim itemMaximumDamage = item.Weapon.MaximumDamage
            If itemMaximumDamage.HasValue Then
                maximumDamage = If(maximumDamage, 0) + itemMaximumDamage.Value
            End If
        Next
        maximumDamage = If(maximumDamage, character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.UnarmedMaximumDamage)).Value)
        Return If(value < 0, 0, If(value > maximumDamage.Value, maximumDamage.Value, value))
    End Function
    ReadOnly Property RollMoneyDrop As Long
        Get
            Return character.CharacterType.Combat.RollMoneyDrop
        End Get
    End Property
    ReadOnly Property XPValue As Long
        Get
            Return character.CharacterType.Combat.XPValue
        End Get
    End Property
    Friend Sub DropLoot()
        'TODO: unequip everything
        For Each item In character.Inventory.Items
            character.Movement.Location.Inventory.Add(item)
        Next
        character.CharacterType.Combat.DropLoot(character.Movement.Location)
    End Sub
    Function Kill(killedBy As ICharacter) As (Sfx?, List(Of String)) Implements ICharacterPhysicalCombat.Kill
        Dim lines As New List(Of String)
        lines.Add($"You kill {character.Name}!")
        Dim sfx As Sfx? = Game.Sfx.EnemyDeath
        Dim money As Long = RollMoneyDrop
        If money > 0 Then
            lines.Add($"You get {money} money!")
            killedBy.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Money), money)
        End If
        Dim xp As Long = XPValue
        If xp > 0 Then
            lines.Add($"You get {xp} XP!")
            If killedBy.Advancement.AddXP(xp) Then
                lines.Add($"You level up!")
            End If
        End If
        DropLoot()
        character.Destroy()
        Return (sfx, lines)
    End Function
    Sub DoCounterAttacks() Implements ICharacterPhysicalCombat.DoCounterAttacks
        Dim enemies = character.Movement.Location.Factions.EnemiesOf(character)
        Dim enemyCount = enemies.Count
        Dim enemyIndex = 1
        For Each enemy In enemies
            DoCounterAttack(enemy, enemyIndex, enemyCount)
            enemyIndex += 1
        Next
    End Sub
    Public Sub Run() Implements ICharacterPhysicalCombat.Run
        If CanFight Then
            character.Movement.Direction = RNG.FromEnumerable(CardinalDirections(WorldData))
            If character.Movement.CanMove(character.Movement.Direction) Then
                character.EnqueueMessage("You successfully ran!") 'TODO: sfx
                character.Movement.Move(character.Movement.Direction)
                Exit Sub
            End If
            character.EnqueueMessage("You fail to run!") 'TODO: shucks!
            DoCounterAttacks()
        End If
    End Sub
    Public Sub Fight() Implements ICharacterPhysicalCombat.Fight
        If CanFight Then
            DoAttack()
            DoCounterAttacks()
        End If
    End Sub
    Private Sub DoAttack()
        Dim lines As New List(Of String)
        Dim attackRoll = RollAttack()
        Dim enemy = character.Movement.Location.Factions.EnemiesOf(character).First
        lines.Add($"You roll an attack of {attackRoll}.")
        Dim defendRoll = enemy.PhysicalCombat.RollDefend()
        lines.Add($"{enemy.Name} rolls a defend of {defendRoll}.")
        Dim result = attackRoll - defendRoll
        Dim sfx As Sfx?
        enemy.DoArmorWear(attackRoll)
        Select Case result
            Case Is <= 0
                lines.Add("You miss!")
                sfx = Game.Sfx.Miss
            Case Else
                Dim damage = DetermineDamage(result)
                lines.Add($"You do {damage} damage!")
                enemy.PhysicalCombat.DoDamage(damage)
                For Each brokenItemType In character.DoWeaponWear(damage)
                    lines.Add($"Yer {brokenItemType.Name} breaks!")
                Next
                If enemy.Health.IsDead Then
                    Dim killResult = enemy.PhysicalCombat.Kill(character)
                    sfx = If(killResult.Item1, sfx)
                    lines.AddRange(killResult.Item2)
                    Exit Select
                End If
                sfx = Game.Sfx.EnemyHit
                lines.Add($"{enemy.Name} has {enemy.Health.Current} HP left.")
        End Select
        character.EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Private Function IsImmobilized() As Boolean
        Return If(character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Immobilization)), 0) > 0
    End Function
    Private Sub DoCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        If character.Health.IsDead Then
            Return
        End If
        If IsImmobilized() Then
            DoImmobilizedTurn(enemy, enemyIndex, enemyCount)
            Return
        End If
        Select Case enemy.CharacterType.Combat.GenerateAttackType
            Case AttackType.Physical
                DoPhysicalCounterAttack(enemy, enemyIndex, enemyCount)
            Case AttackType.Mental
                DoMentalCounterAttack(enemy, enemyIndex, enemyCount)
        End Select
    End Sub
    Private Function GetAttackDice() As Long
        Dim dice = character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Strength)).Value
        For Each entry In EquippedItems
            dice += entry.Weapon.AttackDice
        Next
        Return dice
    End Function
    Private Function GetDefendDice() As Long
        Dim dice = character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Dexterity)).Value
        For Each entry In EquippedItems
            dice += entry.Armor.DefendDice
        Next
        Return dice
    End Function
    Private Sub DoImmobilizedTurn(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        lines.Add($"{enemy.Name} is immobilized!")
        enemy.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Immobilization), -1)
        character.EnqueueMessage(lines.ToArray)
    End Sub
    Private Sub DoMentalCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        lines.Add($"{enemy.Name} attempts to intimidate you!")
        Dim influenceRoll = enemy.MentalCombat.RollInfluence
        lines.Add($"{enemy.Name} rolls influence of {influenceRoll}.")
        Dim willpowerRoll = character.MentalCombat.RollWillpower()
        lines.Add($"You roll willpower of {willpowerRoll}.")
        Dim result = influenceRoll - willpowerRoll
        Dim sfx As Sfx?
        Select Case result
            Case Is <= 0
                lines.Add($"{enemy.Name} fails to intimidate you!")
                sfx = Game.Sfx.Miss
            Case Else
                lines.Add($"{enemy.Name} adds 1 stress!")
                character.MentalCombat.AddStress(1)
                If character.MentalCombat.IsDemoralized() Then
                    lines.Add($"{enemy.Name} completely demoralizes you and you drop everything and run away!")
                    Panic()
                    character.Money \= 2
                    character.Movement.Location = Game.Location.FromLocationType(WorldData, LocationType.FromId(WorldData, 1L)).Single
                    Exit Select
                End If
                lines.Add($"You have {character.MentalCombat.CurrentMP} MP left.")
        End Select
        character.EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Private Sub DoPhysicalCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        Dim attackRoll = enemy.PhysicalCombat.RollAttack()
        lines.Add($"{enemy.Name} rolls an attack of {attackRoll}.")
        For Each brokenItemType In character.DoArmorWear(attackRoll)
            lines.Add($"Yer {brokenItemType.Name} breaks!")
        Next
        Dim defendRoll = RollDefend()
        lines.Add($"You roll a defend of {defendRoll}.")
        Dim result = attackRoll - defendRoll
        Dim sfx As Sfx?
        Select Case result
            Case Is <= 0
                lines.Add($"{enemy.Name} misses!")
                sfx = Game.Sfx.Miss
            Case Else
                Dim damage = enemy.PhysicalCombat.DetermineDamage(result)
                lines.Add($"{enemy.Name} does {damage} damage!")
                DoDamage(damage)
                enemy.DoWeaponWear(damage)
                If character.Health.IsDead Then
                    sfx = Game.Sfx.PlayerDeath
                    lines.Add($"{enemy.Name} kills you!")
                    Dim partingShot = enemy.PhysicalCombat.PartingShot
                    If Not String.IsNullOrEmpty(partingShot) Then
                        lines.Add($"{enemy.Name} says ""{partingShot}""")
                    End If
                    Exit Select
                End If
                sfx = Game.Sfx.PlayerHit
                lines.Add($"You have {character.Health.Current} HP left.")
        End Select
        character.EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Private Sub Panic()
        For Each equipSlot In character.EquippedSlots
            character.Unequip(equipSlot)
        Next
        For Each item In character.Inventory.Items
            character.Movement.Location.Inventory.Add(item)
        Next
    End Sub
End Class