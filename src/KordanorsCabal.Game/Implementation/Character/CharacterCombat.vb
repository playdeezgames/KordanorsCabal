Public Class CharacterCombat
    Inherits BaseThingie
    Implements ICharacterCombat
    Private ReadOnly character As ICharacter

    Public Sub New(worldData As IWorldData, character As ICharacter)
        MyBase.New(worldData, character.Id)
        Me.character = character
    End Sub
    Shared Function FromId(worldData As IWorldData, character As ICharacter) As ICharacterCombat
        Return If(character IsNot Nothing, New CharacterCombat(worldData, character), Nothing)
    End Function
    ReadOnly Property CanFight As Boolean Implements ICharacterCombat.CanFight
        Get
            If character.Location Is Nothing Then
                Return False
            End If
            Return character.Location.Factions.EnemiesOf(character).Any
        End Get
    End Property
    ReadOnly Property IsEnemy(character As ICharacter) As Boolean Implements ICharacterCombat.IsEnemy
        Get
            Return Me.character.CharacterType.Combat.IsEnemy(character.CharacterType)
        End Get
    End Property
    Function RollDefend() As Long Implements ICharacterCombat.RollDefend
        Dim maximumDefend = character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType11)).Value
        Return Math.Min(RollDice(GetDefendDice() + NegativeInfluence()), maximumDefend)
    End Function
    Function RollAttack() As Long Implements ICharacterCombat.RollAttack
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
    ReadOnly Property PartingShot As String Implements ICharacterCombat.PartingShot
        Get
            Return character.CharacterType.Combat.PartingShot
        End Get
    End Property
    Sub DoDamage(damage As Long) Implements ICharacterCombat.DoDamage
        character.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType12), damage)
    End Sub
    ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
    Function DetermineDamage(value As Long) As Long Implements ICharacterCombat.DetermineDamage
        Dim maximumDamage As Long? = Nothing
        For Each item In EquippedItems
            Dim itemMaximumDamage = item.Weapon.MaximumDamage
            If itemMaximumDamage.HasValue Then
                maximumDamage = If(maximumDamage, 0) + itemMaximumDamage.Value
            End If
        Next
        maximumDamage = If(maximumDamage, character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType10)).Value)
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
            character.Location.Inventory.Add(item)
        Next
        character.CharacterType.Combat.DropLoot(character.Location)
    End Sub
    Function Kill(killedBy As ICharacter) As (Sfx?, List(Of String)) Implements ICharacterCombat.Kill
        Dim lines As New List(Of String)
        lines.Add($"You kill {character.Name}!")
        Dim sfx As Sfx? = Game.Sfx.EnemyDeath
        Dim money As Long = RollMoneyDrop
        If money > 0 Then
            lines.Add($"You get {money} money!")
            killedBy.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType14), money)
        End If
        Dim xp As Long = XPValue
        If xp > 0 Then
            lines.Add($"You get {xp} XP!")
            If killedBy.AddXP(xp) Then
                lines.Add($"You level up!")
            End If
        End If
        DropLoot()
        character.Destroy()
        Return (sfx, lines)
    End Function
    Sub DoCounterAttacks() Implements ICharacterCombat.DoCounterAttacks
        Dim enemies = character.Location.Factions.EnemiesOf(character)
        Dim enemyCount = enemies.Count
        Dim enemyIndex = 1
        For Each enemy In enemies
            DoCounterAttack(enemy, enemyIndex, enemyCount)
            enemyIndex += 1
        Next
    End Sub
    Public Sub Run() Implements ICharacterCombat.Run
        If CanFight Then
            character.Direction = RNG.FromEnumerable(CardinalDirections(WorldData))
            If character.Movement.CanMove(character.Direction) Then
                character.EnqueueMessage("You successfully ran!") 'TODO: sfx
                character.Movement.Move(character.Direction)
                Exit Sub
            End If
            character.EnqueueMessage("You fail to run!") 'TODO: shucks!
            DoCounterAttacks()
        End If
    End Sub
    Public Sub Fight() Implements ICharacterCombat.Fight
        If CanFight Then
            DoAttack()
            DoCounterAttacks()
        End If
    End Sub
    Private Sub DoAttack()
        Dim lines As New List(Of String)
        Dim attackRoll = RollAttack()
        Dim enemy = character.Location.Factions.EnemiesOf(character).First
        lines.Add($"You roll an attack of {attackRoll}.")
        Dim defendRoll = enemy.Combat.RollDefend()
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
                enemy.Combat.DoDamage(damage)
                For Each brokenItemType In character.DoWeaponWear(damage)
                    lines.Add($"Yer {brokenItemType.Name} breaks!")
                Next
                If enemy.Health.IsDead Then
                    Dim killResult = enemy.Combat.Kill(character)
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
        Return If(character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType23)), 0) > 0
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
        Dim dice = character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType1)).Value
        For Each entry In EquippedItems
            dice += entry.Weapon.AttackDice
        Next
        Return dice
    End Function
    Private Function GetDefendDice() As Long
        Dim dice = character.GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType2)).Value
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
        enemy.ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.CharacterStatisticType23), -1)
        character.EnqueueMessage(lines.ToArray)
    End Sub
    Private Sub DoMentalCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        lines.Add($"{enemy.Name} attempts to intimidate you!")
        Dim influenceRoll = enemy.RollInfluence
        lines.Add($"{enemy.Name} rolls influence of {influenceRoll}.")
        Dim willpowerRoll = character.RollWillpower()
        lines.Add($"You roll willpower of {willpowerRoll}.")
        Dim result = influenceRoll - willpowerRoll
        Dim sfx As Sfx?
        Select Case result
            Case Is <= 0
                lines.Add($"{enemy.Name} fails to intimidate you!")
                sfx = Game.Sfx.Miss
            Case Else
                lines.Add($"{enemy.Name} adds 1 stress!")
                character.AddStress(1)
                If character.IsDemoralized() Then
                    lines.Add($"{enemy.Name} completely demoralizes you and you drop everything and run away!")
                    Panic()
                    character.Money \= 2
                    character.Location = Game.Location.FromLocationType(WorldData, LocationType.FromId(WorldData, 1L)).Single
                    Exit Select
                End If
                lines.Add($"You have {character.CurrentMP} MP left.")
        End Select
        character.EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Private Sub DoPhysicalCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        Dim attackRoll = enemy.Combat.RollAttack()
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
                Dim damage = enemy.Combat.DetermineDamage(result)
                lines.Add($"{enemy.Name} does {damage} damage!")
                DoDamage(damage)
                enemy.DoWeaponWear(damage)
                If character.Health.IsDead Then
                    sfx = Game.Sfx.PlayerDeath
                    lines.Add($"{enemy.Name} kills you!")
                    Dim partingShot = enemy.Combat.PartingShot
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
            character.Location.Inventory.Add(item)
        Next
    End Sub
End Class
