Public Interface ICharacter
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
    ReadOnly Property Interaction As ICharacterInteraction
    ReadOnly Property Equipment As ICharacterEquipment
    'Messages
    Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String) 'TODO: make an Sfx.None, and make it optional?
    Sub EnqueueMessage(ParamArray lines() As String)
    'Misc
    Property Mode As Long 'TODO: only affects player....
    Sub Destroy()
End Interface