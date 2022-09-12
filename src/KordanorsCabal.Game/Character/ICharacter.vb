Public Interface ICharacter
    Inherits IBaseThingie
    Sub AcceptQuest(quest As Quest)
    Sub AddStress(delta As Long)
    Function AddXP(xp As Long) As Boolean
    Sub AssignPoint(statisticType As ICharacterStatisticType)
    Function CanAcceptQuest(quest As Quest) As Boolean
    ReadOnly Property CanBeBribedWith(itemType As OldItemType) As Boolean
    Function CanCastSpell(spellType As SpellType) As Boolean
    ReadOnly Property CanDoIntimidation() As Boolean
    ReadOnly Property CanFight As Boolean
    ReadOnly Property CanGamble As Boolean
    ReadOnly Property CanIntimidate As Boolean
    ReadOnly Property CharacterType As ICharacterType
    Property CurrentHP As Long
    ReadOnly Property IsDead As Boolean
    ReadOnly Property IsEnemy(character As ICharacter) As Boolean
    ReadOnly Property IsDemoralized As Boolean
    ReadOnly Property IsUndead As Boolean
    Property Location As ILocation
    ReadOnly Property MaximumHP As Long
    ReadOnly Property Name As String
    ReadOnly Property PartingShot As String
    ReadOnly Property MaximumMana As Long
    Property Statistic(statisticType As ICharacterStatisticType) As Long
    ReadOnly Property HasStatistic(statisticType As ICharacterStatisticType) As Boolean
    ReadOnly Property Inventory As IInventory
    Property Hunger As Long
    Property Highness As Long
    Property FoodPoisoning As Long
    Property Drunkenness As Long
    Property CurrentMP As Long
    Property CurrentMana As Long
    Property Chafing As Long
    Property Money As Long
    ReadOnly Property IsEncumbered As Boolean
    Function HasItemType(itemType As OldItemType) As Boolean
    ReadOnly Property NeedsHealing As Boolean
    ReadOnly Property CanInteract As Boolean
    Sub Interact()
    ReadOnly Property HasSpells As Boolean
    Function HasItemsToRepair(shoppeType As ShoppeType) As Boolean
    ReadOnly Property HasEquipment As Boolean
    Function CanMoveForward() As Boolean
    Sub UseItem(item As IItem)
    Sub Unequip(equipSlot As IEquipSlot)
    Function HasVisited(location As ILocation) As Boolean
    ReadOnly Property Spells As IReadOnlyDictionary(Of SpellType, Long)
    Sub Cast(spellType As SpellType)
    ReadOnly Property EquippedSlots As IEnumerable(Of IEquipSlot)
    Function Equipment(equipSlot As IEquipSlot) As IItem
    ReadOnly Property Encumbrance As Long
    Sub Equip(item As IItem)
    ReadOnly Property MaximumEncumbrance As Long
    ReadOnly Property ItemsToRepair(shoppeType As ShoppeType) As IEnumerable(Of IItem)
    ReadOnly Property CanMap() As Boolean
    Sub CompleteQuest(quest As Quest)
    Function CanMoveBackward() As Boolean
    Function CanMoveRight() As Boolean
    Function CanMoveLeft() As Boolean
    Sub Gamble()
    Sub DoIntimidation()
    Sub Fight()
    Sub PurifyItems()
    Property Direction As IDirection
    Function CanMove(direction As IDirection) As Boolean
    Sub Run()
    Function Move(direction As IDirection) As Boolean
    Property Mode As PlayerMode
    ReadOnly Property IsFullyAssigned As Boolean
    Sub SetStatistic(statisticType As ICharacterStatisticType, statisticValue As Long)
    Function GetStatistic(statisticType As ICharacterStatisticType) As Long?
    Sub ChangeStatistic(statisticType As ICharacterStatisticType, delta As Long)
    Function HasQuest(quest As Quest) As Boolean
    Function CanLearn(spellType As SpellType) As Boolean
    Function Kill(killedBy As ICharacter) As (Sfx?, List(Of String))
    Sub Destroy()
    Function DetermineDamage(value As Long) As Long
    Sub DoDamage(damage As Long)
    Function DoArmorWear(wear As Long) As IEnumerable(Of OldItemType)
    Sub DoImmobilization(delta As Long)
    Function DoWeaponWear(wear As Long) As IEnumerable(Of OldItemType)
    Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String)
    Sub EnqueueMessage(ParamArray lines() As String)
    Sub Learn(spellType As SpellType)
    Sub Heal()
    Sub DoFatigue(fatigue As Long)
    Sub DoCounterAttacks()
    Function RollWillpower() As Long
    Function RollDefend() As Long
    Function RollAttack() As Long
    Function RollInfluence() As Long
    Function RollSpellDice(spellType As SpellType) As Long
End Interface
