Public Interface ICharacterSpellbook
    Inherits IBaseThingie
    ReadOnly Property Spells As IReadOnlyDictionary(Of Long, Long)
    Sub Learn(spellType As ISpellType)
    Sub Cast(spellType As ISpellType)
    Function RollSpellDice(spellType As ISpellType) As Long
    Function RollPower() As Long
    Function CanCastSpell(spellType As ISpellType) As Boolean
    Function CanLearn(spellType As ISpellType) As Boolean
    ReadOnly Property HasSpells As Boolean
End Interface
