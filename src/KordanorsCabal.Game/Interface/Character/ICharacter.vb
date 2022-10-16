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
    Sub EnqueueMessage(sfx As Sfx?, ParamArray lines() As String)
    Sub Destroy()
End Interface