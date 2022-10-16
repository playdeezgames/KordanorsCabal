﻿Public Interface ICharacter
    Inherits IBaseThingie
    ReadOnly Property CharacterType As ICharacterType
    ReadOnly Property Movement As ICharacterMovement
    ReadOnly Property Quest As ICharacterQuest
    ReadOnly Property Health As ICharacterHealth
    ReadOnly Property PhysicalCombat As ICharacterPhysicalCombat
    ReadOnly Property Advancement As ICharacterAdvancement
    ReadOnly Property MentalCombat As ICharacterMentalCombat
    ReadOnly Property Spellbook As ICharacterSpellbook
    ReadOnly Property Mana As ICharacterMana
    ReadOnly Property Statistics As ICharacterStatistics
    ReadOnly Property Items As ICharacterItems
    ReadOnly Property Repair As ICharacterRepair
    ReadOnly Property Encumbrance As ICharacterEncumbrance
    ReadOnly Property Statuses As ICharacterStatuses
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
