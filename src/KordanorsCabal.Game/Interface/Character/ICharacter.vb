Public Interface ICharacter
    Inherits IBaseThingie
    ReadOnly Property CharacterType As ICharacterType
    ReadOnly Property Movement As ICharacterMovement
    ReadOnly Property Quest As ICharacterQuest
    ReadOnly Property Health As ICharacterHealth
    ReadOnly Property PhysicalCombat As ICharacterPhysicalCombat
    ReadOnly Property Advancement As ICharacterAdvancement
    'Mental Combat
    Sub AddStress(delta As Long)
    ReadOnly Property CanDoIntimidation() As Boolean
    Sub DoIntimidation()
    ReadOnly Property CanIntimidate As Boolean
    ReadOnly Property IsDemoralized As Boolean
    Property CurrentMP As Long
    Function RollWillpower() As Long
    Function RollInfluence() As Long
    'Spells
    ReadOnly Property Spells As IReadOnlyDictionary(Of Long, Long)
    Sub Learn(spellType As ISpellType)
    Sub Cast(spellType As ISpellType)
    Function RollSpellDice(spellType As ISpellType) As Long
    Function RollPower() As Long
    Function CanCastSpell(spellType As ISpellType) As Boolean
    Function CanLearn(spellType As ISpellType) As Boolean
    ReadOnly Property HasSpells As Boolean
    'Mana
    ReadOnly Property MaximumMana As Long
    Property CurrentMana As Long
    Sub DoFatigue(fatigue As Long)
    'Statistics
    Sub SetStatistic(statisticType As ICharacterStatisticType, statisticValue As Long)
    Function GetStatistic(statisticType As ICharacterStatisticType) As Long?
    Sub ChangeStatistic(statisticType As ICharacterStatisticType, delta As Long)
    'Inventory
    ReadOnly Property Inventory As IInventory
    ReadOnly Property CanBeBribedWith(itemType As IItemType) As Boolean
    Function HasItemType(itemType As IItemType) As Boolean
    Sub UseItem(item As IItem)
    Sub PurifyItems()
    'Repair
    Function HasItemsToRepair(shoppeType As IShoppeType) As Boolean
    ReadOnly Property ItemsToRepair(shoppeType As IShoppeType) As IEnumerable(Of IItem)
    'Encumbrance
    ReadOnly Property IsEncumbered As Boolean
    ReadOnly Property Encumbrance As Long
    ReadOnly Property MaximumEncumbrance As Long
    'Statuses
    ReadOnly Property IsUndead As Boolean
    Property Hunger As Long
    Property Highness As Long
    Property FoodPoisoning As Long
    Property Drunkenness As Long
    Property Chafing As Long
    Property Money As Long
    Sub DoImmobilization(delta As Long)
    'Interaction
    Sub Interact()
    ReadOnly Property CanInteract As Boolean
    'Equipment
    Function DoArmorWear(wear As Long) As IEnumerable(Of IItemType)
    Function DoWeaponWear(wear As Long) As IEnumerable(Of IItemType)
    ReadOnly Property HasEquipment As Boolean
    ReadOnly Property EquippedSlots As IEnumerable(Of IEquipSlot)
    Sub Unequip(equipSlot As IEquipSlot)
    Function Equipment(equipSlot As IEquipSlot) As IItem
    Sub Equip(item As IItem)
    'Gamble
    Sub Gamble()
    ReadOnly Property CanGamble As Boolean
    'Messages
    Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String)
    Sub EnqueueMessage(ParamArray lines() As String)
    'Misc
    ReadOnly Property Name As String
    Property Mode As Long
    Sub Destroy()
End Interface
