﻿Public Class Character
    Inherits BaseThingie
    Implements ICharacter
    Sub New(worldData As IWorldData, characterId As Long)
        MyBase.New(worldData, characterId)
    End Sub
    ReadOnly Property Spells As IReadOnlyDictionary(Of SpellType, Long) Implements ICharacter.Spells
        Get
            Return WorldData.CharacterSpell.
                ReadForCharacter(Id).
                ToDictionary(Function(x) CType(x.Item1, SpellType), Function(x) x.Item2)
        End Get
    End Property

    Public ReadOnly Property ItemsToRepair(shoppeType As ShoppeType) As IEnumerable(Of Item) Implements ICharacter.ItemsToRepair
        Get
            Dim items As New List(Of Item)
            items.AddRange(Inventory.Items.Where(Function(x) x.NeedsRepair))
            items.AddRange(EquippedItems.Where(Function(x) x.NeedsRepair))
            Return items.Where(Function(x) shoppeType.Repairs.ContainsKey(x.ItemType))
        End Get
    End Property

    Sub PurifyItems() Implements ICharacter.PurifyItems
        For Each item In Inventory.Items
            item.Purify()
        Next
        For Each item In EquippedItems
            item.Purify()
        Next
    End Sub

    ReadOnly Property HasSpells As Boolean Implements ICharacter.HasSpells
        Get
            Return Spells.Any
        End Get
    End Property
    ReadOnly Property CharacterType As ICharacterType Implements ICharacter.CharacterType
        Get
            Return Game.CharacterType.FromId(WorldData, WorldData.Character.ReadCharacterType(Id).Value)
        End Get
    End Property
    Public Function HasQuest(quest As Quest) As Boolean Implements ICharacter.HasQuest
        Return WorldData.CharacterQuest.Read(Id, quest)
    End Function
    Property Money As Long Implements ICharacter.Money
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 14L)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 14L), value)
        End Set
    End Property
    Sub Learn(spellType As SpellType) Implements ICharacter.Learn
        If Not CanLearn(spellType) Then
            EnqueueMessage($"You cannot learn {spellType.Name} at this time!")
            Return
        End If
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType), 0) + 1
        EnqueueMessage($"You now know {spellType.Name} at level {nextLevel}.")
        WorldData.CharacterSpell.Write(Id, spellType, nextLevel)
    End Sub
    Function CanLearn(spellType As SpellType) As Boolean Implements ICharacter.CanLearn
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType), 0) + 1
        If nextLevel > spellType.MaximumLevel Then
            Return False
        End If
        Return If(GetStatistic(CharacterStatisticType.FromId(WorldData, 5L)), 0) >= spellType.RequiredPower(nextLevel)
    End Function

    Sub DoImmobilization(delta As Long) Implements ICharacter.DoImmobilization
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, 23L), delta)
    End Sub

    Function RollSpellDice(spellType As SpellType) As Long Implements ICharacter.RollSpellDice
        If Not Spells.ContainsKey(spellType) Then
            Return 0
        End If
        Return RollDice(Power + Spells(SpellType.HolyBolt))
    End Function
    ReadOnly Property Power As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 5L)).Value
        End Get
    End Property

    Public Function HasItemsToRepair(shoppeType As ShoppeType) As Boolean Implements ICharacter.HasItemsToRepair
        Return ItemsToRepair(shoppeType).Any
    End Function

    ReadOnly Property CanBeBribedWith(itemType As OldItemType) As Boolean Implements ICharacter.CanBeBribedWith
        Get
            Return CharacterType.CanBeBribedWith(itemType)
        End Get
    End Property
    ReadOnly Property IsUndead As Boolean Implements ICharacter.IsUndead
        Get
            Return CharacterType.IsUndead
        End Get
    End Property
    Overridable Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String) Implements ICharacter.EnqueueMessage
        'do nothing!
    End Sub
    Overridable Sub EnqueueMessage(ParamArray lines() As String) Implements ICharacter.EnqueueMessage
        'do nothing!
    End Sub
    ReadOnly Property CanFight As Boolean Implements ICharacter.CanFight
        Get
            Return Location.Enemies(Me).Any
        End Get
    End Property
    Friend Shared Function Create(worldData As IWorldData, characterType As ICharacterType, location As ILocation, initialStatistics As IReadOnlyList(Of (ICharacterStatisticType, Long))) As ICharacter
        Dim character = FromId(worldData, worldData.Character.Create(characterType.Id, location.Id))
        For Each entry In initialStatistics
            character.SetStatistic(entry.Item1, entry.Item2)
        Next
        Return character
    End Function
    Public Sub SetStatistic(statisticType As ICharacterStatisticType, statisticValue As Long) Implements ICharacter.SetStatistic
        WorldData.CharacterStatistic.Write(Id, statisticType.Id, Math.Min(Math.Max(statisticValue, statisticType.MinimumValue), statisticType.MaximumValue))
    End Sub
    Sub ChangeStatistic(statisticType As ICharacterStatisticType, delta As Long) Implements ICharacter.ChangeStatistic
        SetStatistic(statisticType, GetStatistic(statisticType).Value + delta)
    End Sub
    Property Location As ILocation Implements ICharacter.Location
        Get
            Return Game.Location.FromId(WorldData, WorldData.Character.ReadLocation(Id).Value)
        End Get
        Set(value As ILocation)
            WorldData.Character.WriteLocation(Id, value.Id)
            WorldData.CharacterLocation.Write(Id, value.Id)
        End Set
    End Property
    Shared Function FromId(worldData As IWorldData, characterId As Long) As ICharacter
        Return New Character(worldData, characterId)
    End Function
    Public Function GetStatistic(statisticType As ICharacterStatisticType) As Long? Implements ICharacter.GetStatistic
        Dim result = If(WorldData.CharacterStatistic.Read(Id,
                                                          statisticType.Id), statisticType.DefaultValue)
        If result.HasValue Then
            For Each item In EquippedItems
                Dim buff As Long = If(item.EquippedBuff(statisticType), 0)
                result = result.Value + buff
            Next
        End If
        Return result
    End Function
    ReadOnly Property Inventory As Inventory Implements ICharacter.Inventory
        Get
            Dim inventoryId As Long? = WorldData.Inventory.ReadForCharacter(Id)
            If Not inventoryId.HasValue Then
                inventoryId = WorldData.Inventory.CreateForCharacter(Id)
            End If
            Return New Inventory(WorldData, inventoryId.Value)
        End Get
    End Property
    ReadOnly Property IsEncumbered As Boolean Implements ICharacter.IsEncumbered
        Get
            Return Encumbrance > MaximumEncumbrance
        End Get
    End Property
    ReadOnly Property Encumbrance As Long Implements ICharacter.Encumbrance
        Get
            Dim result = Inventory.TotalEncumbrance
            For Each item In EquippedItems
                result += item.Encumbrance
            Next
            Return result
        End Get
    End Property
    ReadOnly Property MaximumEncumbrance As Long Implements ICharacter.MaximumEncumbrance
        Get
            Return If(
            GetStatistic(CharacterStatisticType.FromId(WorldData, 24L)), 0) +
            If(
                GetStatistic(CharacterStatisticType.FromId(WorldData, 25L)), 0) *
            If(
                GetStatistic(CharacterStatisticType.FromId(WorldData, 1L)), 0)
        End Get
    End Property
    Public Function HasVisited(location As ILocation) As Boolean Implements ICharacter.HasVisited
        Return WorldData.CharacterLocation.Read(Id, location.Id)
    End Function

    ReadOnly Property IsEnemy(character As ICharacter) As Boolean Implements ICharacter.IsEnemy
        Get
            Return CharacterType.IsEnemy(character.CharacterType)
        End Get
    End Property
    Private Function RollDice(dice As Long) As Long
        Dim result As Long = 0
        While dice > 0
            result += RNG.RollDice("1d6/6")
            dice -= 1
        End While
        Return result
    End Function
    Function RollDefend() As Long Implements ICharacter.RollDefend
        Dim maximumDefend = GetStatistic(CharacterStatisticType.FromId(WorldData, 11L)).Value
        Return Math.Min(RollDice(GetDefendDice() + NegativeInfluence()), maximumDefend)
    End Function
    Private Function GetDefendDice() As Long
        Dim dice = GetStatistic(CharacterStatisticType.FromId(WorldData, 2L)).Value
        For Each entry In EquippedItems
            dice += entry.DefendDice
        Next
        Return dice
    End Function
    Function RollAttack() As Long Implements ICharacter.RollAttack
        Return RollDice(GetAttackDice() + NegativeInfluence())
    End Function
    Private Function NegativeInfluence() As Long
        Return If(Drunkenness > 0 OrElse Highness > 0 OrElse Chafing > 0, -1, 0)
    End Function
    Function RollInfluence() As Long Implements ICharacter.RollInfluence
        Return RollDice(If(GetStatistic(CharacterStatisticType.FromId(WorldData, 3L)), 0) + NegativeInfluence())
    End Function
    Function RollWillpower() As Long Implements ICharacter.RollWillpower
        Return RollDice(If(GetStatistic(CharacterStatisticType.FromId(WorldData, 4L)), 0) + NegativeInfluence())
    End Function
    Private Function GetAttackDice() As Long
        Dim dice = GetStatistic(CharacterStatisticType.FromId(WorldData, 1L)).Value
        For Each entry In EquippedItems
            dice += entry.AttackDice
        Next
        Return dice
    End Function
    ReadOnly Property IsDemoralized() As Boolean Implements ICharacter.IsDemoralized
        Get
            If GetStatistic(CharacterStatisticType.FromId(WorldData, 4L)).HasValue Then
                Return CurrentMP <= 0
            End If
            Return False
        End Get
    End Property
    Sub AddStress(delta As Long) Implements ICharacter.AddStress
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, 13L), delta)
    End Sub
    ReadOnly Property CanIntimidate As Boolean Implements ICharacter.CanIntimidate
        Get
            If Not GetStatistic(CharacterStatisticType.FromId(WorldData, 4L)).HasValue Then
                Return False
            End If
            Return Location.Friends(Me).Count <= Location.Enemies(Me).Count
        End Get
    End Property
    ReadOnly Property Name As String Implements ICharacter.Name
        Get
            Return CharacterType.Name
        End Get
    End Property
    Property CurrentHP As Long Implements ICharacter.CurrentHP
        Get
            Return Math.Max(0, MaximumHP - GetStatistic(CharacterStatisticType.FromId(WorldData, 12L)).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 12L), GetStatistic(CharacterStatisticType.FromId(WorldData, 6L)).Value - value)
        End Set
    End Property
    ReadOnly Property MaximumHP As Long Implements ICharacter.MaximumHP
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 6L)).Value
        End Get
    End Property
    ReadOnly Property PartingShot As String Implements ICharacter.PartingShot
        Get
            Return CharacterType.PartingShot
        End Get
    End Property
    Property CurrentMP As Long Implements ICharacter.CurrentMP
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.FromId(WorldData, 7)).Value - GetStatistic(CharacterStatisticType.FromId(WorldData, 13L)).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 13L), GetStatistic(CharacterStatisticType.FromId(WorldData, 7)).Value - value)
        End Set
    End Property
    Property CurrentMana As Long Implements ICharacter.CurrentMana
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.FromId(WorldData, 8L)).Value - GetStatistic(CharacterStatisticType.FromId(WorldData, 15L)).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 15L), GetStatistic(CharacterStatisticType.FromId(WorldData, 8L)).Value - value)
        End Set
    End Property
    Sub DoDamage(damage As Long) Implements ICharacter.DoDamage
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, 12L), damage)
    End Sub
    Sub DoFatigue(fatigue As Long) Implements ICharacter.DoFatigue
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, 15L), fatigue)
    End Sub
    Sub Destroy() Implements ICharacter.Destroy
        WorldData.Character.Clear(Id)
    End Sub
    ReadOnly Property IsDead As Boolean Implements ICharacter.IsDead
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 12L)).Value >= GetStatistic(CharacterStatisticType.FromId(WorldData, 6L)).Value
        End Get
    End Property
    Function DetermineDamage(value As Long) As Long Implements ICharacter.DetermineDamage
        Dim maximumDamage As Long? = Nothing
        For Each item In EquippedItems
            Dim itemMaximumDamage = item.MaximumDamage
            If itemMaximumDamage.HasValue Then
                maximumDamage = If(maximumDamage, 0) + itemMaximumDamage.Value
            End If
        Next
        maximumDamage = If(maximumDamage, GetStatistic(CharacterStatisticType.FromId(WorldData, 10L)).Value)
        Return If(value < 0, 0, If(value > maximumDamage.Value, maximumDamage.Value, value))
    End Function
    ReadOnly Property RollMoneyDrop As Long
        Get
            Return CharacterType.RollMoneyDrop
        End Get
    End Property
    ReadOnly Property XPValue As Long
        Get
            Return CharacterType.XPValue
        End Get
    End Property
    ReadOnly Property NeedsHealing As Boolean Implements ICharacter.NeedsHealing
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 12L)).Value > 0
        End Get
    End Property
    Function DoWeaponWear(wear As Long) As IEnumerable(Of OldItemType) Implements ICharacter.DoWeaponWear
        Dim result As New List(Of OldItemType)
        While wear > 0
            Dim brokenItemType = WearOneWeapon()
            If brokenItemType.HasValue Then
                result.Add(brokenItemType.Value)
            End If
            wear -= 1
        End While
        Return result
    End Function
    Function DoArmorWear(wear As Long) As IEnumerable(Of OldItemType) Implements ICharacter.DoArmorWear
        Dim result As New List(Of OldItemType)
        While wear > 0
            Dim brokenItemType = WearOneArmor()
            If brokenItemType.HasValue Then
                result.Add(brokenItemType.Value)
            End If
            wear -= 1
        End While
        Return result
    End Function
    Private Function WearOneWeapon() As OldItemType?
        Dim items = EquippedItems.Where(Function(x) x.MaximumDurability IsNot Nothing AndAlso x.IsWeapon).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.ReduceDurability(1)
            If item.IsBroken Then
                WearOneWeapon = item.ItemType
                item.Destroy()
            End If
        End If
    End Function
    Private Function WearOneArmor() As OldItemType?
        Dim items = EquippedItems.Where(Function(x) x.MaximumDurability IsNot Nothing AndAlso x.IsArmor).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.ReduceDurability(1)
            If item.IsBroken Then
                WearOneArmor = item.ItemType
                item.Destroy()
            End If
        End If
    End Function
    Friend Sub DropLoot()
        'TODO: unequip everything
        For Each item In Inventory.Items
            Location.Inventory.Add(item)
        Next
        CharacterType.DropLoot(Location)
    End Sub
    Function Equipment(equipSlot As EquipSlot) As Item Implements ICharacter.Equipment
        Return Item.FromId(WorldData, WorldData.CharacterEquipSlot.ReadForCharacterEquipSlot(Id, equipSlot.Id))
    End Function
    ReadOnly Property EquippedItems As IEnumerable(Of Item)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
    ReadOnly Property EquippedSlots As IEnumerable(Of EquipSlot) Implements ICharacter.EquippedSlots
        Get
            Return WorldData.CharacterEquipSlot.ReadEquipSlotsForCharacter(Id).Select(Function(x) New EquipSlot(WorldData, x))
        End Get
    End Property
    Function AddXP(xp As Long) As Boolean Implements ICharacter.AddXP
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, 16L), xp)
        Dim xpGoal = GetStatistic(CharacterStatisticType.FromId(WorldData, 17L)).Value
        If GetStatistic(CharacterStatisticType.FromId(WorldData, 16L)).Value >= xpGoal Then
            ChangeStatistic(CharacterStatisticType.FromId(WorldData, 16L), -xpGoal)
            ChangeStatistic(CharacterStatisticType.FromId(WorldData, 17L), xpGoal)
            ChangeStatistic(CharacterStatisticType.FromId(WorldData, 9L), 1)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 12L), 0)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 13L), 0)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 15L), 0)
            Return True
        End If
        Return False
    End Function
    Function Kill(killedBy As ICharacter) As (Sfx?, List(Of String)) Implements ICharacter.Kill
        Dim lines As New List(Of String)
        lines.Add($"You kill {Name}!")
        Dim sfx As Sfx? = Game.Sfx.EnemyDeath
        Dim money As Long = RollMoneyDrop
        If money > 0 Then
            lines.Add($"You get {money} money!")
            killedBy.ChangeStatistic(CharacterStatisticType.FromId(WorldData, 14L), money)
        End If
        Dim xp As Long = XPValue
        If xp > 0 Then
            lines.Add($"You get {xp} XP!")
            If killedBy.AddXP(xp) Then
                lines.Add($"You level up!")
            End If
        End If
        DropLoot()
        Destroy()
        Return (sfx, lines)
    End Function
    Sub DoCounterAttacks() Implements ICharacter.DoCounterAttacks
        Dim enemies = Location.Enemies(Me)
        Dim enemyCount = enemies.Count
        Dim enemyIndex = 1
        For Each enemy In enemies
            DoCounterAttack(enemy, enemyIndex, enemyCount)
            enemyIndex += 1
        Next
    End Sub
    Private Sub DoCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        If IsDead Then
            Return
        End If
        If IsImmobilized() Then
            DoImmobilizedTurn(enemy, enemyIndex, enemyCount)
            Return
        End If
        Select Case enemy.CharacterType.GenerateAttackType
            Case AttackType.Physical
                DoPhysicalCounterAttack(enemy, enemyIndex, enemyCount)
            Case AttackType.Mental
                DoMentalCounterAttack(enemy, enemyIndex, enemyCount)
        End Select
    End Sub

    Private Sub DoImmobilizedTurn(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        lines.Add($"{enemy.Name} is immobilized!")
        enemy.ChangeStatistic(CharacterStatisticType.FromId(WorldData, 23L), -1)
        EnqueueMessage(lines.ToArray)
    End Sub

    Private Function IsImmobilized() As Boolean
        Return If(GetStatistic(CharacterStatisticType.FromId(WorldData, 23L)), 0) > 0
    End Function

    Private Sub DoMentalCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        lines.Add($"{enemy.Name} attempts to intimidate you!")
        Dim influenceRoll = enemy.RollInfluence
        lines.Add($"{enemy.Name} rolls influence of {influenceRoll}.")
        Dim willpowerRoll = RollWillpower()
        lines.Add($"You roll willpower of {willpowerRoll}.")
        Dim result = influenceRoll - willpowerRoll
        Dim sfx As Sfx?
        Select Case result
            Case Is <= 0
                lines.Add($"{enemy.Name} fails to intimidate you!")
                sfx = Game.Sfx.Miss
            Case Else
                lines.Add($"{enemy.Name} adds 1 stress!")
                AddStress(1)
                If IsDemoralized() Then
                    lines.Add($"{enemy.Name} completely demoralizes you and you drop everything and run away!")
                    Panic()
                    Money \= 2
                    Location = Game.Location.FromLocationType(WorldData, LocationType.FromId(WorldData, 1L)).Single
                    Exit Select
                End If
                lines.Add($"You have {CurrentMP} MP left.")
        End Select
        EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Public Sub Unequip(equipSlot As EquipSlot) Implements ICharacter.Unequip
        Dim item = Equipment(equipSlot)
        If item IsNot Nothing Then
            Inventory.Add(item)
            WorldData.CharacterEquipSlot.Clear(Id, equipSlot.Id)
        End If
    End Sub
    Private Sub Panic()
        For Each equipSlot In EquippedSlots
            Unequip(equipSlot)
        Next
        For Each item In Inventory.Items
            Location.Inventory.Add(item)
        Next
    End Sub
    Private Sub DoPhysicalCounterAttack(enemy As ICharacter, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        Dim attackRoll = enemy.RollAttack()
        lines.Add($"{enemy.Name} rolls an attack of {attackRoll}.")
        For Each brokenItemType In DoArmorWear(attackRoll)
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
                Dim damage = enemy.DetermineDamage(result)
                lines.Add($"{enemy.Name} does {damage} damage!")
                DoDamage(damage)
                enemy.DoWeaponWear(damage)
                If IsDead Then
                    sfx = Game.Sfx.PlayerDeath
                    lines.Add($"{enemy.Name} kills you!")
                    Dim partingShot = enemy.PartingShot
                    If Not String.IsNullOrEmpty(partingShot) Then
                        lines.Add($"{enemy.Name} says ""{partingShot}""")
                    End If
                    Exit Select
                End If
                sfx = Game.Sfx.PlayerHit
                lines.Add($"You have {CurrentHP} HP left.")
        End Select
        EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Function HasItemType(itemType As OldItemType) As Boolean Implements ICharacter.HasItemType
        Return Inventory.ItemsOfType(itemType).Any
    End Function
    Property Drunkenness As Long Implements ICharacter.Drunkenness
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 18L)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 18L), value)
        End Set
    End Property
    Property Chafing As Long Implements ICharacter.Chafing
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 22L)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 22L), value)
        End Set
    End Property
    Property Highness As Long Implements ICharacter.Highness
        Get
            Return If(GetStatistic(CharacterStatisticType.FromId(WorldData, 19L)), 0)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 19L), value)
        End Set
    End Property
    Property Hunger As Long Implements ICharacter.Hunger
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 20L)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 20L), value)
        End Set
    End Property
    Public ReadOnly Property MaximumMana As Long Implements ICharacter.MaximumMana
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 8L)).Value
        End Get
    End Property
    Public Property FoodPoisoning As Long Implements ICharacter.FoodPoisoning
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, 21L)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, 21L), value)
        End Set
    End Property
    ReadOnly Property IsFullyAssigned As Boolean Implements ICharacter.IsFullyAssigned
        Get
            Return If(GetStatistic(CharacterStatisticType.FromId(WorldData, 9L)), 0) = 0
        End Get
    End Property
    Public Sub AssignPoint(statisticType As ICharacterStatisticType) Implements ICharacter.AssignPoint
        If Not IsFullyAssigned Then
            ChangeStatistic(statisticType, 1)
            ChangeStatistic(CharacterStatisticType.FromId(WorldData, 9L), -1)
        End If
    End Sub
    Public ReadOnly Property CanGamble As Boolean Implements ICharacter.CanGamble
        Get
            Return Money >= 5
        End Get
    End Property
    Public Property Direction As IDirection Implements ICharacter.Direction
        Get
            Return New Direction(WorldData, WorldData.Player.ReadDirection().Value)
        End Get
        Set(value As IDirection)
            WorldData.Player.WriteDirection(value.Id)
        End Set
    End Property
    Public ReadOnly Property CanCast() As Boolean
        Get
            Return Spells.Keys.Any(Function(x) CanCastSpell(x))
        End Get
    End Property
    Public Function CanCastSpell(spellType As SpellType) As Boolean Implements ICharacter.CanCastSpell
        Return spellType.CanCast(Me)
    End Function
    Public Sub Gamble() Implements ICharacter.Gamble
        If Not CanGamble Then
            EnqueueMessage("You cannot gamble at this time!")
            Return
        End If
        Dim lines As New List(Of String)
        lines.Add("You flip the two coins!")
        Dim firstCoin = RNG.FromRange(0, 1)
        lines.Add($"The first coin comes up {If(firstCoin > 0, "heads", "tails")}!")
        Dim secondCoin = RNG.FromRange(0, 1)
        lines.Add($"The second coin comes up {If(secondCoin > 0, "heads", "tails")}!")
        Dim winner = firstCoin > 0 AndAlso secondCoin > 0
        If winner Then
            lines.Add("You win and receive 15 money!")
            Money += 15
        Else
            lines.Add("You lose and must pay 5 money!")
            Money -= 5
        End If
        'TODO: sound effect
        EnqueueMessage(lines.ToArray)
    End Sub
    Public Sub Cast(spellType As SpellType) Implements ICharacter.Cast
        If Not CanCastSpell(spellType) Then
            EnqueueMessage($"You cannot cast {spellType.Name} at this time.")
            Return
        End If
        spellType.Cast(Me)
    End Sub
    Public Sub Equip(item As Item) Implements ICharacter.Equip
        If item.CanEquip Then
            WorldData.InventoryItem.ClearForItem(item.Id)
            Dim equipSlots = item.EquipSlots
            Dim availableEquipSlots = equipSlots.Where(Function(x) Equipment(x) Is Nothing)
            Dim equipSlot = If(availableEquipSlots.Any, availableEquipSlots.First, equipSlots.First)
            Dim oldItem = Equipment(equipSlot)
            If oldItem IsNot Nothing Then
                Inventory.Add(oldItem)
            End If
            WorldData.CharacterEquipSlot.Write(Id, equipSlot.Id, item.Id)
            EnqueueMessage($"You equip {item.Name} to {equipSlot.Name}.")
            Return
        End If
        EnqueueMessage($"You cannot equip {item.Name}!")
    End Sub
    Public Function CanAcceptQuest(quest As Quest) As Boolean Implements ICharacter.CanAcceptQuest
        Return Not HasQuest(quest) AndAlso quest.CanAccept(Me)
    End Function
    Public Sub UseItem(item As Item) Implements ICharacter.UseItem
        If item.CanUse(Me) Then
            item.Use(Me)
            If item.IsConsumed Then
                item.Destroy()
            End If
        End If
    End Sub
    Public Property Mode As PlayerMode Implements ICharacter.Mode
        Get
            Return CType(WorldData.Player.ReadPlayerMode().Value, PlayerMode)
        End Get
        Set(value As PlayerMode)
            WorldData.Player.WritePlayerMode(value)
        End Set
    End Property
    Public Sub CompleteQuest(quest As Quest) Implements ICharacter.CompleteQuest
        quest.Complete(Me)
    End Sub
    Public Sub AcceptQuest(quest As Quest) Implements ICharacter.AcceptQuest
        quest.Accept(Me)
    End Sub
    Public ReadOnly Property CanInteract As Boolean Implements ICharacter.CanInteract
        Get
            Return (Location?.Feature?.Id).HasValue
        End Get
    End Property
    Public Function GetItemTypeCount(itemType As OldItemType) As Integer
        Return Inventory.Items.Where(Function(x) x.ItemType = itemType).Count
    End Function
    Public Function CanMoveLeft() As Boolean Implements ICharacter.CanMoveLeft
        Return CanMove(Direction.PreviousDirection)
    End Function
    Public ReadOnly Property CanMap() As Boolean Implements ICharacter.CanMap
        Get
            Return Location.CanMap
        End Get
    End Property
    Public Function CanMoveRight() As Boolean Implements ICharacter.CanMoveRight
        Return CanMove(Direction.NextDirection)
    End Function
    Public Function CanMoveForward() As Boolean Implements ICharacter.CanMoveForward
        Return CanMove(Direction)
    End Function
    Public Sub Heal() Implements ICharacter.Heal
        SetStatistic(CharacterStatisticType.FromId(WorldData, 12L), 0)
    End Sub
    Public Function CanMoveBackward() As Boolean Implements ICharacter.CanMoveBackward
        Return CanMove(Direction.Opposite)
    End Function
    Public Sub Interact() Implements ICharacter.Interact
        If CanInteract Then
            Mode = Location.Feature.InteractionMode()
        End If
    End Sub
    ReadOnly Property CanDoIntimidation() As Boolean Implements ICharacter.CanDoIntimidation
        Get
            If If(GetStatistic(CharacterStatisticType.FromId(WorldData, 3L)), 0) <= 0 Then
                Return False
            End If
            Dim enemy = Location.Enemies(Me).FirstOrDefault
            If enemy Is Nothing Then
                Return False
            End If
            Return enemy.CanIntimidate
        End Get
    End Property
    Public Sub DoIntimidation() Implements ICharacter.DoIntimidation
        If CanDoIntimidation Then
            Dim lines As New List(Of String)
            Dim enemy = Location.Enemies(Me).First
            Dim influenceRoll = RollInfluence()
            lines.Add($"You roll {influenceRoll} influence.")
            Dim willpowerRoll = enemy.RollWillpower()
            lines.Add($"{enemy.Name} rolls {willpowerRoll} willpower.")
            If influenceRoll > willpowerRoll Then
                enemy.AddStress(1)
                lines.Add($"{enemy.Name} loses 1 MP!")
                If enemy.IsDemoralized Then
                    lines.Add($"{enemy.Name} runs away!")
                    enemy.Destroy()
                End If
            Else
                lines.Add($"{enemy.Name} is not intimidated.")
            End If
            EnqueueMessage(lines.ToArray)
            DoCounterAttacks()
            Return
        End If
        EnqueueMessage("You cannot intimidate at this time!")
    End Sub
    Public Function CanMove(direction As IDirection) As Boolean Implements ICharacter.CanMove
        If IsEncumbered Then
            Return False
        End If
        If Not Location.HasRoute(direction) Then
            Return False
        End If
        If Not Location.Routes(direction).CanMove(Me) Then
            Return False
        End If
        If Location.Routes(direction).ToLocation.RequiresMP AndAlso IsDemoralized() Then
            Return False
        End If
        Return True
    End Function
    Public Sub Run() Implements ICharacter.Run
        If CanFight Then
            Direction = RNG.FromEnumerable(CardinalDirections(WorldData))
            If CanMove(Direction) Then
                EnqueueMessage("You successfully ran!") 'TODO: sfx
                Move(Direction)
                Exit Sub
            End If
            EnqueueMessage("You fail to run!") 'TODO: shucks!
            DoCounterAttacks()
        End If
    End Sub
    Public Function Move(direction As IDirection) As Boolean Implements ICharacter.Move
        If CanMove(direction) Then
            Dim hungerRate = Math.Max(Highness \ 2 + FoodPoisoning \ 2, 1)
            Hunger += hungerRate
            Drunkenness -= 1
            Highness -= 1
            FoodPoisoning -= 1
            Chafing -= 1
            Location = Location.Routes(direction).Move(Me)
            If Hunger = CharacterStatisticType.FromId(WorldData, 20L).MaximumValue Then
                Hunger \= 2
                CurrentHP -= 1
                Return True
            End If
        End If
        Return False
    End Function
    Public Sub Fight() Implements ICharacter.Fight
        If CanFight Then
            DoAttack()
            DoCounterAttacks()
        End If
    End Sub
    Private Sub DoAttack()
        Dim lines As New List(Of String)
        Dim attackRoll = RollAttack()
        Dim enemy = Location.Enemies(Me).First
        lines.Add($"You roll an attack of {attackRoll}.")
        Dim defendRoll = enemy.RollDefend()
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
                enemy.DoDamage(damage)
                For Each brokenItemType In DoWeaponWear(damage)
                    lines.Add($"Yer {brokenItemType.Name} breaks!")
                Next
                If enemy.IsDead Then
                    Dim killResult = enemy.Kill(Me)
                    sfx = If(killResult.Item1, sfx)
                    lines.AddRange(killResult.Item2)
                    Exit Select
                End If
                sfx = Game.Sfx.EnemyHit
                lines.Add($"{enemy.Name} has {enemy.CurrentHP} HP left.")
        End Select
        EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Public ReadOnly Property HasEquipment As Boolean Implements ICharacter.HasEquipment
        Get
            Return EquippedItems.Any
        End Get
    End Property

    Public Property Statistic(statisticType As CharacterStatisticType) As Long Implements ICharacter.Statistic
        Get
            Return GetStatistic(statisticType).Value
        End Get
        Set(value As Long)
            SetStatistic(statisticType, value)
        End Set
    End Property

    Public ReadOnly Property HasStatistic(statisticType As CharacterStatisticType) As Boolean Implements ICharacter.HasStatistic
        Get
            Return GetStatistic(statisticType).HasValue
        End Get
    End Property
End Class
