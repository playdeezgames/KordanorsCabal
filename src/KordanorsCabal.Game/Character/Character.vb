Public Class Character
    ReadOnly Property Id As Long
    Sub New(characterId As Long)
        Id = characterId
    End Sub
    ReadOnly Property Spells As IReadOnlyDictionary(Of SpellType, Long)
        Get
            Return StaticWorldData.World.CharacterSpell.
                ReadForCharacter(Id).
                ToDictionary(Function(x) CType(x.Item1, SpellType), Function(x) x.Item2)
        End Get
    End Property

    Public ReadOnly Property ItemsToRepair(shoppeType As ShoppeType) As IEnumerable(Of Item)
        Get
            Dim items As New List(Of Item)
            items.AddRange(Inventory.Items.Where(Function(x) x.NeedsRepair))
            items.AddRange(EquippedItems.Where(Function(x) x.NeedsRepair))
            Return items.Where(Function(x) shoppeType.Repairs.ContainsKey(x.ItemType))
        End Get
    End Property

    Friend Sub PurifyItems()
        For Each item In Inventory.Items
            item.Purify()
        Next
        For Each item In EquippedItems
            item.Purify()
        Next
    End Sub

    ReadOnly Property HasSpells As Boolean
        Get
            Return Spells.Any
        End Get
    End Property
    ReadOnly Property CharacterType As CharacterType
        Get
            Return CType(StaticWorldData.World.Character.ReadCharacterType(Id).Value, CharacterType)
        End Get
    End Property
    Public Function HasQuest(quest As Quest) As Boolean
        Return StaticWorldData.World.CharacterQuest.Exists(Id, quest)
    End Function
    Property Money As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Money)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Money), value)
        End Set
    End Property
    Friend Sub Learn(spellType As SpellType)
        If Not CanLearn(spellType) Then
            EnqueueMessage($"You cannot learn {spellType.Name} at this time!")
            Return
        End If
        Dim nextLevel = If(StaticWorldData.World.CharacterSpell.Read(Id, spellType), 0) + 1
        EnqueueMessage($"You now know {spellType.Name} at level {nextLevel}.")
        StaticWorldData.World.CharacterSpell.Write(Id, spellType, nextLevel)
    End Sub
    Friend Function CanLearn(spellType As SpellType) As Boolean
        Dim nextLevel = If(StaticWorldData.World.CharacterSpell.Read(Id, spellType), 0) + 1
        If nextLevel > spellType.MaximumLevel Then
            Return False
        End If
        Return If(GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Power)), 0) >= spellType.RequiredPower(nextLevel)
    End Function

    Friend Sub DoImmobilization(delta As Long)
        ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Immobilization), delta)
    End Sub

    Friend Function RollSpellDice(spellType As SpellType) As Long
        If Not Spells.ContainsKey(spellType) Then
            Return 0
        End If
        Return RollDice(Power + Spells(SpellType.HolyBolt))
    End Function
    ReadOnly Property Power As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Power)).Value
        End Get
    End Property

    Public Function HasItemsToRepair(shoppeType As ShoppeType) As Boolean
        Return ItemsToRepair(shoppeType).Any
    End Function

    Friend Function CanBeBribedWith(itemType As ItemType) As Boolean
        Return CharacterType.CanBeBribedWith(itemType)
    End Function
    ReadOnly Property IsUndead As Boolean
        Get
            Return CharacterType.IsUndead
        End Get
    End Property
    Overridable Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String)
        'do nothing!
    End Sub
    Overridable Sub EnqueueMessage(ParamArray lines() As String)
        'do nothing!
    End Sub
    ReadOnly Property CanFight As Boolean
        Get
            Return Location.Enemies(Me).Any
        End Get
    End Property
    Friend Shared Function Create(characterType As CharacterType, location As Location) As Character
        Dim character = FromId(StaticWorldData.World.Character.Create(characterType, location.Id))
        For Each entry In characterType.InitialStatistics
            character.SetStatistic(New CharacterStatisticType(entry.Key), entry.Value)
        Next
        Return character
    End Function
    Public Sub SetStatistic(statisticType As CharacterStatisticType, statisticValue As Long)
        StaticWorldData.World.CharacterStatistic.Write(Id, statisticType.Id, Math.Min(Math.Max(statisticValue, statisticType.MinimumValue), statisticType.MaximumValue))
    End Sub
    Friend Sub ChangeStatistic(statisticType As CharacterStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType).Value + delta)
    End Sub
    Property Location As Location
        Get
            Return Location.FromId(StaticWorldData.World.Character.ReadLocation(Id).Value)
        End Get
        Set(value As Location)
            StaticWorldData.World.Character.WriteLocation(Id, value.Id)
            StaticWorldData.World.CharacterLocation.Write(Id, value.Id)
        End Set
    End Property
    Shared Function FromId(characterId As Long) As Character
        Return New Character(characterId)
    End Function
    Public Function GetStatistic(statisticType As CharacterStatisticType) As Long?
        Dim result = If(StaticWorldData.World.CharacterStatistic.Read(Id, statisticType.Id), statisticType.DefaultValue)
        If result.HasValue Then
            For Each item In EquippedItems
                Dim buff As Long = If(item.EquippedBuff(statisticType), 0)
                result = result.Value + buff
            Next
        End If
        Return result
    End Function
    ReadOnly Property Inventory As Inventory
        Get
            Dim inventoryId As Long? = StaticWorldData.World.Inventory.ReadForCharacter(Id)
            If Not inventoryId.HasValue Then
                inventoryId = StaticWorldData.World.Inventory.CreateForCharacter(Id)
            End If
            Return New Inventory(inventoryId.Value)
        End Get
    End Property
    ReadOnly Property IsEncumbered As Boolean
        Get
            Return Encumbrance > MaximumEncumbrance
        End Get
    End Property
    ReadOnly Property Encumbrance As Long
        Get
            Dim result = Inventory.TotalEncumbrance
            For Each item In EquippedItems
                result += item.Encumbrance
            Next
            Return result
        End Get
    End Property
    ReadOnly Property MaximumEncumbrance As Long
        Get
            Return CharacterType.MaximumEncumbrance(Me)
        End Get
    End Property
    Public Function HasVisited(location As Location) As Boolean
        Return StaticWorldData.World.CharacterLocation.Read(Id, location.Id)
    End Function

    Friend Function IsEnemy(character As Character) As Boolean
        Return CharacterType.IsEnemy(character)
    End Function
    Private Function RollDice(dice As Long) As Long
        Dim result As Long = 0
        While dice > 0
            result += RNG.RollDice("1d6/6")
            dice -= 1
        End While
        Return result
    End Function
    Friend Function RollDefend() As Long
        Dim maximumDefend = GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.BaseMaximumDefend)).Value
        Return Math.Min(RollDice(GetDefendDice() + NegativeInfluence()), maximumDefend)
    End Function
    Private Function GetDefendDice() As Long
        Dim dice = GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Dexterity)).Value
        For Each entry In EquippedItems
            dice += entry.DefendDice
        Next
        Return dice
    End Function
    Friend Function RollAttack() As Long
        Return RollDice(GetAttackDice() + NegativeInfluence())
    End Function
    Private Function NegativeInfluence() As Long
        Return If(Drunkenness > 0 OrElse Highness > 0 OrElse Chafing > 0, -1, 0)
    End Function
    Friend Function RollInfluence() As Long
        Return RollDice(If(GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Influence)), 0) + NegativeInfluence())
    End Function
    Friend Function RollWillpower() As Long
        Return RollDice(If(GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Willpower)), 0) + NegativeInfluence())
    End Function
    Private Function GetAttackDice() As Long
        Dim dice = GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Strength)).Value
        For Each entry In EquippedItems
            dice += entry.AttackDice
        Next
        Return dice
    End Function
    Friend Function IsDemoralized() As Boolean
        If GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Willpower)).HasValue Then
            Return CurrentMP <= 0
        End If
        Return False
    End Function
    Friend Sub AddStress(delta As Long)
        ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Stress), delta)
    End Sub
    ReadOnly Property CanIntimidate As Boolean
        Get
            If Not GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Willpower)).HasValue Then
                Return False
            End If
            Return Location.Friends(Me).Count <= Location.Enemies(Me).Count
        End Get
    End Property
    ReadOnly Property Name As String
        Get
            Return CharacterType.Name
        End Get
    End Property
    Property CurrentHP As Long
        Get
            Return Math.Max(0, MaximumHP - GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds)).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds), GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.HP)).Value - value)
        End Set
    End Property
    ReadOnly Property MaximumHP As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.HP)).Value
        End Get
    End Property
    ReadOnly Property PartingShot As String
        Get
            Return CharacterType.PartingShot
        End Get
    End Property
    Property CurrentMP As Long
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.FromName(MP)).Value - GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Stress)).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Stress), GetStatistic(CharacterStatisticType.FromName(MP)).Value - value)
        End Set
    End Property
    Property CurrentMana As Long
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Mana)).Value - GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Fatigue)).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Fatigue), GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Mana)).Value - value)
        End Set
    End Property
    Friend Sub DoDamage(damage As Long)
        ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds), damage)
    End Sub
    Friend Sub DoFatigue(fatigue As Long)
        ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Fatigue), fatigue)
    End Sub
    Friend Sub Destroy()
        StaticWorldData.World.Character.Clear(Id)
    End Sub
    ReadOnly Property IsDead As Boolean
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds)).Value >= GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.HP)).Value
        End Get
    End Property
    Function DetermineDamage(value As Long) As Long
        Dim maximumDamage As Long? = Nothing
        For Each item In EquippedItems
            Dim itemMaximumDamage = item.MaximumDamage
            If itemMaximumDamage.HasValue Then
                maximumDamage = If(maximumDamage, 0) + itemMaximumDamage.Value
            End If
        Next
        maximumDamage = If(maximumDamage, GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.UnarmedMaximumDamage)).Value)
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
    ReadOnly Property NeedsHealing As Boolean
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds)).Value > 0
        End Get
    End Property
    Friend Function DoWeaponWear(wear As Long) As IEnumerable(Of ItemType)
        Dim result As New List(Of ItemType)
        While wear > 0
            Dim brokenItemType = WearOneWeapon()
            If brokenItemType.HasValue Then
                result.Add(brokenItemType.Value)
            End If
            wear -= 1
        End While
        Return result
    End Function
    Friend Function DoArmorWear(wear As Long) As IEnumerable(Of ItemType)
        Dim result As New List(Of ItemType)
        While wear > 0
            Dim brokenItemType = WearOneArmor()
            If brokenItemType.HasValue Then
                result.Add(brokenItemType.Value)
            End If
            wear -= 1
        End While
        Return result
    End Function
    Private Function WearOneWeapon() As ItemType?
        Dim items = EquippedItems.Where(Function(x) x.MaximumDurability IsNot Nothing AndAlso x.IsWeapon)
        If items.Any Then
            Dim item = RNG.FromEnumerable(items)
            item.ReduceDurability(1)
            If item.IsBroken Then
                WearOneWeapon = item.ItemType
                item.Destroy()
            End If
        End If
    End Function
    Private Function WearOneArmor() As ItemType?
        Dim items = EquippedItems.Where(Function(x) x.MaximumDurability IsNot Nothing AndAlso x.IsArmor)
        If items.Any Then
            Dim item = RNG.FromEnumerable(items)
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
    Function Equipment(equipSlot As EquipSlot) As Item
        Return Item.FromId(StaticWorldData.World.CharacterEquipSlot.ReadForCharacterEquipSlot(Id, equipSlot.Id))
    End Function
    ReadOnly Property EquippedItems As IEnumerable(Of Item)
        Get
            Return StaticWorldData.World.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(x))
        End Get
    End Property
    ReadOnly Property EquippedSlots As IEnumerable(Of EquipSlot)
        Get
            Return StaticWorldData.World.CharacterEquipSlot.ReadEquipSlotsForCharacter(Id).Select(Function(x) New EquipSlot(x))
        End Get
    End Property
    Friend Function AddXP(xp As Long) As Boolean
        ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XP), xp)
        Dim xpGoal = GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XPGoal)).Value
        If GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XP)).Value >= xpGoal Then
            ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XP), -xpGoal)
            ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.XPGoal), xpGoal)
            ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned), 1)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds), 0)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Stress), 0)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Fatigue), 0)
            Return True
        End If
        Return False
    End Function
    Friend Function Kill(killedBy As Character) As (Sfx?, List(Of String))
        Dim lines As New List(Of String)
        lines.Add($"You kill {Name}!")
        Dim sfx As Sfx? = Game.Sfx.EnemyDeath
        Dim money As Long = RollMoneyDrop
        If money > 0 Then
            lines.Add($"You get {money} money!")
            killedBy.ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Money), money)
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
    Friend Sub DoCounterAttacks()
        Dim enemies = Location.Enemies(Me)
        Dim enemyCount = enemies.Count
        Dim enemyIndex = 1
        For Each enemy In enemies
            DoCounterAttack(enemy, enemyIndex, enemyCount)
            enemyIndex += 1
        Next
    End Sub
    Private Sub DoCounterAttack(enemy As Character, enemyIndex As Integer, enemyCount As Integer)
        If IsDead Then
            Return
        End If
        If IsImmobilized() Then
            DoImmobilizedTurn(enemy, enemyIndex, enemyCount)
            Return
        End If
        Select Case enemy.CharacterType.GenerateAttackType(Me)
            Case AttackType.Physical
                DoPhysicalCounterAttack(enemy, enemyIndex, enemyCount)
            Case AttackType.Mental
                DoMentalCounterAttack(enemy, enemyIndex, enemyCount)
        End Select
    End Sub

    Private Sub DoImmobilizedTurn(enemy As Character, enemyIndex As Integer, enemyCount As Integer)
        Dim lines As New List(Of String) From {
            $"Counter-attack {enemyIndex}/{enemyCount}:"
        }
        lines.Add($"{enemy.Name} is immobilized!")
        enemy.ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Immobilization), -1)
        EnqueueMessage(lines.ToArray)
    End Sub

    Private Function IsImmobilized() As Boolean
        Return If(GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Immobilization)), 0) > 0
    End Function

    Private Sub DoMentalCounterAttack(enemy As Character, enemyIndex As Integer, enemyCount As Integer)
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
                    Location = Game.Location.FromLocationType(LocationType.FromName(TownSquare)).Single
                    Exit Select
                End If
                lines.Add($"You have {CurrentMP} MP left.")
        End Select
        EnqueueMessage(sfx, lines.ToArray)
    End Sub
    Public Sub Unequip(equipSlot As EquipSlot)
        Dim item = Equipment(equipSlot)
        If item IsNot Nothing Then
            Inventory.Add(item)
            StaticWorldData.World.CharacterEquipSlot.Clear(Id, equipSlot.Id)
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
    Private Sub DoPhysicalCounterAttack(enemy As Character, enemyIndex As Integer, enemyCount As Integer)
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
    Function HasItemType(itemType As ItemType) As Boolean
        Return Inventory.ItemsOfType(itemType).Any
    End Function
    Property Drunkenness As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Drunkenness)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Drunkenness), value)
        End Set
    End Property
    Property Chafing As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Chafing)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Chafing), value)
        End Set
    End Property
    Property Highness As Long
        Get
            Return If(GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Highness)), 0)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Highness), value)
        End Set
    End Property
    Property Hunger As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Hunger)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Hunger), value)
        End Set
    End Property
    Public ReadOnly Property MaximumMana As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Mana)).Value
        End Get
    End Property
    Public Property FoodPoisoning As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.FoodPoisoning)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.FoodPoisoning), value)
        End Set
    End Property
    ReadOnly Property IsFullyAssigned As Boolean
        Get
            Return If(GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned)), 0) = 0
        End Get
    End Property
    Public Sub AssignPoint(statisticType As CharacterStatisticType)
        If Not IsFullyAssigned Then
            ChangeStatistic(statisticType, 1)
            ChangeStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Unassigned), -1)
        End If
    End Sub
    Public ReadOnly Property CanGamble As Boolean
        Get
            Return Money >= 5
        End Get
    End Property
    Public Property Direction As Direction
        Get
            Return New Direction(StaticWorldData.World.Player.ReadDirection().Value)
        End Get
        Set(value As Direction)
            StaticWorldData.World.Player.WriteDirection(value.Id)
        End Set
    End Property
    Public ReadOnly Property CanCast() As Boolean
        Get
            Return Spells.Keys.Any(Function(x) CanCastSpell(x))
        End Get
    End Property
    Public Function CanCastSpell(spellType As SpellType) As Boolean
        Return spellType.CanCast(Me)
    End Function
    Public Sub Gamble()
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
    Public Sub Cast(spellType As SpellType)
        If Not CanCastSpell(spellType) Then
            EnqueueMessage($"You cannot cast {spellType.Name} at this time.")
            Return
        End If
        spellType.Cast(Me)
    End Sub
    Public Sub Equip(item As Item)
        If item.CanEquip Then
            StaticWorldData.World.InventoryItem.ClearForItem(item.Id)
            Dim equipSlots = item.EquipSlots
            Dim availableEquipSlots = equipSlots.Where(Function(x) Equipment(x) Is Nothing)
            Dim equipSlot = If(availableEquipSlots.Any, availableEquipSlots.First, equipSlots.First)
            Dim oldItem = Equipment(equipSlot)
            If oldItem IsNot Nothing Then
                Inventory.Add(oldItem)
            End If
            StaticWorldData.World.CharacterEquipSlot.Write(Id, equipSlot.Id, item.Id)
            EnqueueMessage($"You equip {item.Name} to {equipSlot.Name}.")
            Return
        End If
        EnqueueMessage($"You cannot equip {item.Name}!")
    End Sub
    Public Function CanAcceptQuest(quest As Quest) As Boolean
        Return Not HasQuest(quest) AndAlso quest.CanAccept(Me)
    End Function
    Public Sub UseItem(item As Item)
        If item.CanUse(Me) Then
            item.Use(Me)
            If item.IsConsumed Then
                item.Destroy()
            End If
        End If
    End Sub
    Public Property Mode As PlayerMode
        Get
            Return CType(StaticWorldData.World.Player.ReadMode().Value, PlayerMode)
        End Get
        Set(value As PlayerMode)
            StaticWorldData.World.Player.WriteMode(value)
        End Set
    End Property
    Public Sub CompleteQuest(quest As Quest)
        quest.Complete(Me)
    End Sub
    Public Sub AcceptQuest(quest As Quest)
        quest.Accept(Me)
    End Sub
    Public ReadOnly Property CanInteract As Boolean
        Get
            Return (Location?.Feature?.Id).HasValue
        End Get
    End Property
    Public Function GetItemTypeCount(itemType As ItemType) As Integer
        Return Inventory.Items.Where(Function(x) x.ItemType = itemType).Count
    End Function
    Public Function CanMoveLeft() As Boolean
        Return CanMove(Direction.PreviousDirection)
    End Function
    Public ReadOnly Property CanMap() As Boolean
        Get
            Return Location.CanMap
        End Get
    End Property
    Public Function CanMoveRight() As Boolean
        Return CanMove(Direction.NextDirection)
    End Function
    Public Function CanMoveForward() As Boolean
        Return CanMove(Direction)
    End Function
    Public Sub Heal()
        SetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Wounds), 0)
    End Sub
    Public Function CanMoveBackward() As Boolean
        Return CanMove(Direction.Opposite)
    End Function
    Public Sub Interact()
        If CanInteract Then
            Mode = Location.Feature.InteractionMode()
        End If
    End Sub
    ReadOnly Property CanDoIntimidation() As Boolean
        Get
            If If(GetStatistic(CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Influence)), 0) <= 0 Then
                Return False
            End If
            Dim enemy = Location.Enemies(Me).FirstOrDefault
            If enemy Is Nothing Then
                Return False
            End If
            Return enemy.CanIntimidate
        End Get
    End Property
    Public Sub DoIntimidation()
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
    Public Function CanMove(direction As Direction) As Boolean
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
    Public Sub Run()
        If CanFight Then
            Direction = RNG.FromEnumerable(CardinalDirections)
            If CanMove(Direction) Then
                EnqueueMessage("You successfully ran!") 'TODO: sfx
                Move(Direction)
                Exit Sub
            End If
            EnqueueMessage("You fail to run!") 'TODO: shucks!
            DoCounterAttacks()
        End If
    End Sub
    Public Function Move(direction As Direction) As Boolean
        If CanMove(direction) Then
            Dim hungerRate = Math.Max(Highness \ 2 + FoodPoisoning \ 2, 1)
            Hunger += hungerRate
            Drunkenness -= 1
            Highness -= 1
            FoodPoisoning -= 1
            Chafing -= 1
            Location = Location.Routes(direction).Move(Me)
            If Hunger = CharacterStatisticType.FromName(CharacterStatisticTypeUtility.Hunger).MaximumValue Then
                Hunger \= 2
                CurrentHP -= 1
                Return True
            End If
        End If
        Return False
    End Function
    Public Sub Fight()
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
    Public ReadOnly Property HasEquipment As Boolean
        Get
            Return EquippedItems.Any
        End Get
    End Property
End Class
