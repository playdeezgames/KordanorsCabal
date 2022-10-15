Public Class Character
    Inherits BaseThingie
    Implements ICharacter
    Sub New(worldData As IWorldData, characterId As Long)
        MyBase.New(worldData, characterId)
    End Sub
    ReadOnly Property Spells As IReadOnlyDictionary(Of Long, Long) Implements ICharacter.Spells
        Get
            Return WorldData.CharacterSpell.
                ReadForCharacter(Id).
                ToDictionary(Function(x) x.Item1, Function(x) x.Item2)
        End Get
    End Property

    Public ReadOnly Property ItemsToRepair(shoppeType As IShoppeType) As IEnumerable(Of IItem) Implements ICharacter.ItemsToRepair
        Get
            Dim items As New List(Of IItem)
            items.AddRange(Inventory.Items.Where(Function(x) x.Repair.IsNeeded))
            items.AddRange(EquippedItems.Where(Function(x) x.Repair.IsNeeded))
            Return items.Where(Function(x) shoppeType.WillRepair(x.ItemType))
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
            Dim result = WorldData.Character.ReadCharacterType(Id)
            If result Is Nothing Then
                Return Nothing
            End If
            Return Game.CharacterType.FromId(WorldData, result.Value)
        End Get
    End Property
    Property Money As Long Implements ICharacter.Money
        Get
            Return If(GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Money)), 0L)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Money), value)
        End Set
    End Property
    Sub Learn(spellType As ISpellType) Implements ICharacter.Learn
        If Not CanLearn(spellType) Then
            EnqueueMessage($"You cannot learn {spellType.Name} at this time!")
            Return
        End If
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType.Id), 0) + 1
        EnqueueMessage($"You now know {spellType.Name} at level {nextLevel}.")
        WorldData.CharacterSpell.Write(Id, spellType.Id, nextLevel)
    End Sub
    Function CanLearn(spellType As ISpellType) As Boolean Implements ICharacter.CanLearn
        Dim nextLevel = If(WorldData.CharacterSpell.Read(Id, spellType.Id), 0) + 1
        If nextLevel > spellType.MaximumLevel Then
            Return False
        End If
        Return If(GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Power)), 0) >= spellType.RequiredPower(nextLevel)
    End Function
    Sub DoImmobilization(delta As Long) Implements ICharacter.DoImmobilization
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Immobilization), delta)
    End Sub
    Function RollSpellDice(spellType As ISpellType) As Long Implements ICharacter.RollSpellDice
        If Not Spells.ContainsKey(spellType.Id) Then
            Return 0
        End If
        Return RollDice(Power + Spells(spellType.Id))
    End Function
    ReadOnly Property Power As Long
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Power)).Value
        End Get
    End Property
    Public Function HasItemsToRepair(shoppeType As IShoppeType) As Boolean Implements ICharacter.HasItemsToRepair
        Return ItemsToRepair(shoppeType).Any
    End Function
    ReadOnly Property CanBeBribedWith(itemType As IItemType) As Boolean Implements ICharacter.CanBeBribedWith
        Get
            Return CharacterType.Combat.CanBeBribedWith(itemType)
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
    Friend Shared Function Create(worldData As IWorldData, characterType As ICharacterType, location As ILocation, initialStatistics As IReadOnlyList(Of (ICharacterStatisticType, Long))) As ICharacter
        Dim character = FromId(worldData, worldData.Character.Create(characterType.Id, location.Id))
        If initialStatistics IsNot Nothing Then
            For Each entry In initialStatistics
                character.SetStatistic(entry.Item1, entry.Item2)
            Next
        End If
        Return character
    End Function
    Public Sub SetStatistic(statisticType As ICharacterStatisticType, statisticValue As Long) Implements ICharacter.SetStatistic
        WorldData.CharacterStatistic.Write(Id, statisticType.Id, Math.Min(Math.Max(statisticValue, statisticType.MinimumValue), statisticType.MaximumValue))
    End Sub
    Sub ChangeStatistic(statisticType As ICharacterStatisticType, delta As Long) Implements ICharacter.ChangeStatistic
        Dim current = GetStatistic(statisticType)
        If current IsNot Nothing Then
            SetStatistic(statisticType, current.Value + delta)
        End If
    End Sub
    Shared Function FromId(worldData As IWorldData, characterId As Long?) As ICharacter
        Return If(characterId.HasValue, New Character(worldData, characterId.Value), Nothing)
    End Function
    Public Function GetStatistic(statisticType As ICharacterStatisticType) As Long? Implements ICharacter.GetStatistic
        Dim result = If(WorldData.CharacterStatistic.Read(Id,
                                                          statisticType.Id), statisticType.DefaultValue)
        If result.HasValue Then
            For Each item In EquippedItems
                Dim buff As Long = If(item.Equipment.EquippedBuff(statisticType), 0)
                result = result.Value + buff
            Next
        End If
        Return result
    End Function
    ReadOnly Property Inventory As IInventory Implements ICharacter.Inventory
        Get
            Dim inventoryId As Long? = WorldData.Inventory.ReadForCharacter(Id)
            If Not inventoryId.HasValue Then
                inventoryId = WorldData.Inventory.CreateForCharacter(Id)
            End If
            Return Game.Inventory.FromId(WorldData, inventoryId.Value)
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
                result += item.ItemType.Encumbrance
            Next
            Return result
        End Get
    End Property
    ReadOnly Property MaximumEncumbrance As Long Implements ICharacter.MaximumEncumbrance
        Get
            Return If(
            GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.BaseLift)), 0) +
            If(
                GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.BonusLift)), 0) *
            If(
                GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Strength)), 0)
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
    Private Function NegativeInfluence() As Long
        Return If(Drunkenness > 0 OrElse Highness > 0 OrElse Chafing > 0, -1, 0)
    End Function
    Function RollInfluence() As Long Implements ICharacter.RollInfluence
        Return RollDice(If(GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Influence)), 0) + NegativeInfluence())
    End Function
    Function RollWillpower() As Long Implements ICharacter.RollWillpower
        Return RollDice(If(GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Willpower)), 0) + NegativeInfluence())
    End Function
    ReadOnly Property IsDemoralized() As Boolean Implements ICharacter.IsDemoralized
        Get
            If GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Willpower)).HasValue Then
                Return CurrentMP <= 0
            End If
            Return False
        End Get
    End Property
    Sub AddStress(delta As Long) Implements ICharacter.AddStress
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Stress), delta)
    End Sub
    ReadOnly Property CanIntimidate As Boolean Implements ICharacter.CanIntimidate
        Get
            If Not GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Willpower)).HasValue Then
                Return False
            End If
            Return Movement.Location.Factions.AlliesOf(Me).Count <= Movement.Location.Factions.EnemiesOf(Me).Count
        End Get
    End Property
    ReadOnly Property Name As String Implements ICharacter.Name
        Get
            Return CharacterType.Name
        End Get
    End Property
    Property CurrentMP As Long Implements ICharacter.CurrentMP
        Get
            Return Math.Max(0, GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.MP)).Value - GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Stress)).Value)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Stress), GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.MP)).Value - value)
        End Set
    End Property
    Property CurrentMana As Long Implements ICharacter.CurrentMana
        Get
            Dim maximumMana = GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Mana))
            Dim fatigue = GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Fatigue))
            Return Math.Max(0, If(maximumMana, 0L) - If(fatigue, 0L))
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Fatigue), GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Mana)).Value - value)
        End Set
    End Property
    Sub DoFatigue(fatigue As Long) Implements ICharacter.DoFatigue
        ChangeStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Fatigue), fatigue)
    End Sub
    Sub Destroy() Implements ICharacter.Destroy
        WorldData.Character.Clear(Id)
    End Sub
    Function DoWeaponWear(wear As Long) As IEnumerable(Of IItemType) Implements ICharacter.DoWeaponWear
        Dim result As New List(Of IItemType)
        While wear > 0
            Dim brokenItemType = WearOneWeapon()
            If brokenItemType IsNot Nothing Then
                result.Add(brokenItemType)
            End If
            wear -= 1
        End While
        Return result
    End Function
    Function DoArmorWear(wear As Long) As IEnumerable(Of IItemType) Implements ICharacter.DoArmorWear
        Dim result As New List(Of IItemType)
        While wear > 0
            Dim brokenItemType = WearOneArmor()
            If brokenItemType.HasValue Then
                result.Add(ItemType.FromId(WorldData, brokenItemType.Value))
            End If
            wear -= 1
        End While
        Return result
    End Function
    Private Function WearOneWeapon() As IItemType
        WearOneWeapon = Nothing
        Dim items = EquippedItems.Where(Function(x) x.Durability.Maximum IsNot Nothing AndAlso x.Weapon.IsWeapon).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.Durability.Reduce(1)
            If item.Durability.IsBroken Then
                WearOneWeapon = item.ItemType
                item.Destroy()
            End If
        End If
    End Function
    Private Function WearOneArmor() As Long?
        Dim items = EquippedItems.Where(Function(x) x.Durability.Maximum IsNot Nothing AndAlso x.Armor.IsArmor).ToList
        If items.Any Then
            Dim item = RNG.FromList(items)
            item.Durability.Reduce(1)
            If item.Durability.IsBroken Then
                WearOneArmor = If(item.ItemType IsNot Nothing, item.ItemType.Id, Nothing)
                item.Destroy()
            End If
        End If
    End Function
    Function Equipment(equipSlot As IEquipSlot) As IItem Implements ICharacter.Equipment
        Return Item.FromId(WorldData, WorldData.CharacterEquipSlot.ReadForCharacterEquipSlot(Id, equipSlot.Id))
    End Function
    ReadOnly Property EquippedItems As IEnumerable(Of IItem)
        Get
            Return WorldData.CharacterEquipSlot.ReadItemsForCharacter(Id).Select(Function(x) Item.FromId(WorldData, x))
        End Get
    End Property
    ReadOnly Property EquippedSlots As IEnumerable(Of IEquipSlot) Implements ICharacter.EquippedSlots
        Get
            Return WorldData.CharacterEquipSlot.ReadEquipSlotsForCharacter(Id).Select(Function(x) New EquipSlot(WorldData, x))
        End Get
    End Property
    Public Sub Unequip(equipSlot As IEquipSlot) Implements ICharacter.Unequip
        Dim item = Equipment(equipSlot)
        If item IsNot Nothing Then
            Inventory.Add(item)
            WorldData.CharacterEquipSlot.Clear(Id, equipSlot.Id)
        End If
    End Sub
    Function HasItemType(itemType As IItemType) As Boolean Implements ICharacter.HasItemType
        Return Inventory.ItemsOfType(itemType).Any
    End Function
    Property Drunkenness As Long Implements ICharacter.Drunkenness
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Drunkenness)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Drunkenness), value)
        End Set
    End Property
    Property Chafing As Long Implements ICharacter.Chafing
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Chafing)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Chafing), value)
        End Set
    End Property
    Property Highness As Long Implements ICharacter.Highness
        Get
            Return If(GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Highness)), 0)
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Highness), value)
        End Set
    End Property
    Property Hunger As Long Implements ICharacter.Hunger
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Hunger)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Hunger), value)
        End Set
    End Property
    Public ReadOnly Property MaximumMana As Long Implements ICharacter.MaximumMana
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Mana)).Value
        End Get
    End Property
    Public Property FoodPoisoning As Long Implements ICharacter.FoodPoisoning
        Get
            Return GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.FoodPoisoning)).Value
        End Get
        Set(value As Long)
            SetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.FoodPoisoning), value)
        End Set
    End Property
    Public ReadOnly Property CanGamble As Boolean Implements ICharacter.CanGamble
        Get
            Return Money >= 5
        End Get
    End Property
    Public Function CanCastSpell(spellType As ISpellType) As Boolean Implements ICharacter.CanCastSpell
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
    Public Sub Cast(spellType As ISpellType) Implements ICharacter.Cast
        If Not CanCastSpell(spellType) Then
            EnqueueMessage($"You cannot cast {spellType.Name} at this time.")
            Return
        End If
        spellType.Cast(Me)
    End Sub
    Public Sub Equip(item As IItem) Implements ICharacter.Equip
        If item.Equipment.CanEquip Then
            WorldData.InventoryItem.ClearForItem(item.Id)
            Dim equipSlots = item.Equipment.EquipSlots
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
    Public Sub UseItem(item As IItem) Implements ICharacter.UseItem
        If item.Usage.CanUse(Me) Then
            item.Usage.Use(Me)
            If item.Usage.IsConsumed Then
                item.Destroy()
            End If
        End If
    End Sub
    Public Property Mode As Long Implements ICharacter.Mode
        Get
            Return WorldData.Player.ReadPlayerMode().Value
        End Get
        Set(value As Long)
            WorldData.Player.WritePlayerMode(value)
        End Set
    End Property
    Public ReadOnly Property CanInteract As Boolean Implements ICharacter.CanInteract
        Get
            Return (Movement.Location?.Feature?.Id).HasValue
        End Get
    End Property
    Public Sub Interact() Implements ICharacter.Interact
        If CanInteract Then
            Mode = Movement.Location.Feature.InteractionMode()
        End If
    End Sub
    ReadOnly Property CanDoIntimidation() As Boolean Implements ICharacter.CanDoIntimidation
        Get
            If If(GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Influence)), 0) <= 0 Then
                Return False
            End If
            Dim enemy = Movement.Location.Factions.EnemiesOf(Me).FirstOrDefault
            If enemy Is Nothing Then
                Return False
            End If
            Return enemy.CanIntimidate
        End Get
    End Property
    Public Sub DoIntimidation() Implements ICharacter.DoIntimidation
        If CanDoIntimidation Then
            Dim lines As New List(Of String)
            Dim enemy = Movement.Location.Factions.EnemiesOf(Me).First
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
            Combat.DoCounterAttacks()
            Return
        End If
        EnqueueMessage("You cannot intimidate at this time!")
    End Sub

    Public Function RollPower() As Long Implements ICharacter.RollPower
        Return RollDice(If(GetStatistic(CharacterStatisticType.FromId(WorldData, Constants.StatisticTypes.Power)), 0) + NegativeInfluence())
    End Function

    Public ReadOnly Property HasEquipment As Boolean Implements ICharacter.HasEquipment
        Get
            Return EquippedItems.Any
        End Get
    End Property
    Public ReadOnly Property Movement As ICharacterMovement Implements ICharacter.Movement
        Get
            Return CharacterMovement.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Quest As ICharacterQuest Implements ICharacter.Quest
        Get
            Return CharacterQuest.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Health As ICharacterHealth Implements ICharacter.Health
        Get
            Return CharacterHealth.FromId(WorldData, Id)
        End Get
    End Property

    Public ReadOnly Property Combat As ICharacterPhysicalCombat Implements ICharacter.Combat
        Get
            Return CharacterPhysicalCombat.FromCharacter(WorldData, Me)
        End Get
    End Property

    Public ReadOnly Property Advancement As ICharacterAdvancement Implements ICharacter.Advancement
        Get
            Return CharacterAdvancement.FromCharacter(WorldData, Me)
        End Get
    End Property
End Class
