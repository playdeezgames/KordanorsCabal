Public Class Character
    ReadOnly Property Id As Long
    Sub New(characterId As Long)
        Id = characterId
    End Sub

    ReadOnly Property Spells As IReadOnlyDictionary(Of SpellType, Long)
        Get
            Return CharacterSpellData.
                ReadForCharacter(Id).
                ToDictionary(Function(x) CType(x.Item1, SpellType), Function(x) x.Item2)
        End Get
    End Property

    ReadOnly Property HasSpells As Boolean
        Get
            Return Spells.Any
        End Get
    End Property

    ReadOnly Property CharacterType As CharacterType
        Get
            Return CType(CharacterData.ReadCharacterType(Id).Value, CharacterType)
        End Get
    End Property

    Public Function HasQuest(quest As Quest) As Boolean
        Return CharacterQuestData.Exists(Id, quest)
    End Function

    Property Money As Long
        Get
            Return GetStatistic(CharacterStatisticType.Money).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.Money, value)
        End Set
    End Property

    Friend Sub Learn(spellType As SpellType)
        If Not CanLearn(spellType) Then
            EnqueueMessage($"You cannot learn {spellType.Name} at this time!")
            Return
        End If
        Dim nextLevel = If(CharacterSpellData.Read(Id, spellType), 0) + 1
        EnqueueMessage($"You now know {spellType.Name} at level {nextLevel}.")
        CharacterSpellData.Write(Id, spellType, nextLevel)
    End Sub

    Friend Function CanLearn(spellType As SpellType) As Boolean
        Dim nextLevel = If(CharacterSpellData.Read(Id, spellType), 0) + 1
        If nextLevel > spellType.MaximumLevel Then
            Return False
        End If
        Return If(GetStatistic(CharacterStatisticType.Power), 0) >= spellType.RequiredPower(nextLevel)
    End Function

    Friend Function RollSpellDice(spellType As SpellType) As Long
        If Not Spells.ContainsKey(spellType) Then
            Return 0
        End If
        Return RollDice(Power + Spells(SpellType.HolyBolt))
    End Function

    ReadOnly Property Power As Long
        Get
            Return GetStatistic(CharacterStatisticType.Power).Value
        End Get
    End Property

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
        Dim character = FromId(CharacterData.Create(characterType, location.Id))
        For Each entry In characterType.InitialStatistics
            character.SetStatistic(entry.Key, entry.Value)
        Next
        Return character
    End Function

    Public Sub SetStatistic(statisticType As CharacterStatisticType, statisticValue As Long)
        CharacterStatisticData.Write(Id, statisticType, Math.Min(Math.Max(statisticValue, statisticType.MinimumValue), statisticType.MaximumValue))
    End Sub

    Friend Sub ChangeStatistic(statisticType As CharacterStatisticType, delta As Long)
        SetStatistic(statisticType, GetStatistic(statisticType).Value + delta)
    End Sub

    Property Location As Location
        Get
            Return Location.FromId(CharacterData.ReadLocation(Id).Value)
        End Get
        Set(value As Location)
            CharacterData.WriteLocation(Id, value.Id)
            CharacterLocationData.Write(Id, value.Id)
        End Set
    End Property
    Shared Function FromId(characterId As Long) As Character
        Return New Character(characterId)
    End Function

    Public Function GetStatistic(statisticType As CharacterStatisticType) As Long?
        Return If(CharacterStatisticData.Read(Id, statisticType), statisticType.DefaultValue)
    End Function
    ReadOnly Property Inventory As Inventory
        Get
            Dim inventoryId As Long? = InventoryData.ReadForCharacter(Id)
            If Not inventoryId.HasValue Then
                inventoryId = InventoryData.CreateForCharacter(Id)
            End If
            Return New Inventory(inventoryId.Value)
        End Get
    End Property

    ReadOnly Property IsEncumbered As Boolean
        Get
            Return Encumbrance > MaximumEncumbrance
        End Get
    End Property
    ReadOnly Property Encumbrance As Single
        Get
            Dim result = Inventory.TotalEncumbrance
            For Each item In Equipment.Values
                result += item.Encumbrance
            Next
            Return result
        End Get
    End Property
    ReadOnly Property MaximumEncumbrance As Single
        Get
            Return CharacterType.MaximumEncumbrance(Me)
        End Get
    End Property
    Public Function HasVisited(location As Location) As Boolean
        Return CharacterLocationData.Read(Id, location.Id)
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
        Dim maximumDefend = GetStatistic(CharacterStatisticType.BaseMaximumDefend).Value
        Return Math.Min(RollDice(GetDefendDice() + NegativeInfluence()), maximumDefend)
    End Function

    Private Function GetDefendDice() As Long
        Dim dice = GetStatistic(CharacterStatisticType.Dexterity).Value
        For Each entry In Equipment
            dice += entry.Value.DefendDice
        Next
        Return dice
    End Function

    Friend Function RollAttack() As Long
        Return RollDice(GetAttackDice() + NegativeInfluence())
    End Function

    Private Function NegativeInfluence() As Long
        Return If(Drunkenness > 0 OrElse Highness > 0, -1, 0)
    End Function

    Friend Function RollInfluence() As Long
        Return RollDice(If(GetStatistic(CharacterStatisticType.Influence), 0) + NegativeInfluence())
    End Function

    Friend Function RollWillpower() As Long
        Return RollDice(If(GetStatistic(CharacterStatisticType.Willpower), 0) + NegativeInfluence())
    End Function

    Private Function GetAttackDice() As Long
        Dim dice = GetStatistic(CharacterStatisticType.Strength).Value
        For Each entry In Equipment
            dice += entry.Value.AttackDice
        Next
        Return dice
    End Function

    Friend Function IsDemoralized() As Boolean
        If GetStatistic(CharacterStatisticType.Willpower).HasValue Then
            Return CurrentMP <= 0
        End If
        Return False
    End Function

    Friend Sub AddStress(delta As Long)
        ChangeStatistic(CharacterStatisticType.Stress, delta)
    End Sub

    ReadOnly Property CanIntimidate As Boolean
        Get
            If Not GetStatistic(CharacterStatisticType.Willpower).HasValue Then
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

    ReadOnly Property CurrentHP As Long
        Get
            Return Math.Max(0, MaximumHP - GetStatistic(CharacterStatisticType.Wounds).Value)
        End Get
    End Property

    ReadOnly Property MaximumHP As Long
        Get
            Return GetStatistic(CharacterStatisticType.HP).Value
        End Get
    End Property

    ReadOnly Property PartingShot As String
        Get
            Return CharacterType.PartingShot
        End Get
    End Property

    Property CurrentMP As Long
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.MP).Value - GetStatistic(CharacterStatisticType.Stress).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.Stress, GetStatistic(CharacterStatisticType.MP).Value - value)
        End Set
    End Property

    Property CurrentMana As Long
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.Mana).Value - GetStatistic(CharacterStatisticType.Fatigue).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.Fatigue, GetStatistic(CharacterStatisticType.Mana).Value - value)
        End Set
    End Property


    Friend Sub DoDamage(damage As Long)
        ChangeStatistic(CharacterStatisticType.Wounds, damage)
    End Sub

    Friend Sub DoFatigue(fatigue As Long)
        ChangeStatistic(CharacterStatisticType.Fatigue, fatigue)
    End Sub

    Friend Sub Destroy()
        CharacterData.Clear(Id)
    End Sub

    ReadOnly Property IsDead As Boolean
        Get
            Return GetStatistic(CharacterStatisticType.Wounds).Value >= GetStatistic(CharacterStatisticType.HP).Value
        End Get
    End Property

    Function DetermineDamage(value As Long) As Long
        Dim maximumDamage As Long? = Nothing
        For Each entry In Equipment
            Dim item = entry.Value
            Dim itemMaximumDamage = item.MaximumDamage
            If itemMaximumDamage.HasValue Then
                maximumDamage = If(maximumDamage, 0) + itemMaximumDamage.Value
            End If
        Next
        maximumDamage = If(maximumDamage, GetStatistic(CharacterStatisticType.UnarmedMaximumDamage).Value)
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
            Return GetStatistic(CharacterStatisticType.Wounds).Value > 0
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
        Dim items = Equipment.Values.Where(Function(x) x.MaximumDurability IsNot Nothing AndAlso x.IsWeapon)
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
        Dim items = Equipment.Values.Where(Function(x) x.MaximumDurability IsNot Nothing AndAlso x.IsArmor)
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

    ReadOnly Property Equipment As IReadOnlyDictionary(Of EquipSlot, Item)
        Get
            Return CharacterEquipSlotData.Read(Id).
                ToDictionary(
                    Function(x) CType(x.Item1, EquipSlot),
                    Function(x) Item.FromId(x.Item2))
        End Get
    End Property

    Friend Function AddXP(xp As Long) As Boolean
        ChangeStatistic(CharacterStatisticType.XP, xp)
        Dim xpGoal = GetStatistic(CharacterStatisticType.XPGoal).Value
        If GetStatistic(CharacterStatisticType.XP).Value >= xpGoal Then
            ChangeStatistic(CharacterStatisticType.XP, -xpGoal)
            ChangeStatistic(CharacterStatisticType.XPGoal, xpGoal)
            ChangeStatistic(CharacterStatisticType.Unassigned, 1)
            SetStatistic(CharacterStatisticType.Wounds, 0)
            SetStatistic(CharacterStatisticType.Stress, 0)
            SetStatistic(CharacterStatisticType.Fatigue, 0)
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
            killedBy.ChangeStatistic(CharacterStatisticType.Money, money)
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
        Select Case enemy.CharacterType.GenerateAttackType(Me)
            Case AttackType.Physical
                DoPhysicalCounterAttack(enemy, enemyIndex, enemyCount)
            Case AttackType.Mental
                DoMentalCounterAttack(enemy, enemyIndex, enemyCount)
        End Select
    End Sub

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
                    Location = Game.Location.FromLocationType(LocationType.TownSquare).Single
                    Exit Select
                End If
                lines.Add($"You have {CurrentMP} MP left.")
        End Select
        EnqueueMessage(sfx, lines.ToArray)
    End Sub

    Public Sub Unequip(equipSlot As EquipSlot)
        If Equipment.ContainsKey(equipSlot) Then
            Dim item = Equipment(equipSlot)
            Inventory.Add(item)
            CharacterEquipSlotData.Clear(Id, equipSlot)
        End If
    End Sub

    Private Sub Panic()
        For Each equipSlot In Equipment.Keys
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
            Return GetStatistic(CharacterStatisticType.Drunkenness).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.Drunkenness, value)
        End Set
    End Property

    Property Highness As Long
        Get
            Return GetStatistic(CharacterStatisticType.Highness).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.Highness, value)
        End Set
    End Property

    Property Hunger As Long
        Get
            Return GetStatistic(CharacterStatisticType.Hunger).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.Hunger, value)
        End Set
    End Property

    Public ReadOnly Property MaximumMana As Long
        Get
            Return GetStatistic(CharacterStatisticType.Mana).Value
        End Get
    End Property
End Class
