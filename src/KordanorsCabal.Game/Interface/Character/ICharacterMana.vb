Public Interface ICharacterMana
    Inherits IBaseThingie
    ReadOnly Property MaximumMana As Long
    Property CurrentMana As Long
    Sub DoFatigue(fatigue As Long)
End Interface
