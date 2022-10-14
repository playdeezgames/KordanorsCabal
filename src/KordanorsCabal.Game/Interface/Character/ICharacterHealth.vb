Public Interface ICharacterHealth
    Inherits IBaseThingie
    Property CurrentHP As Long
    ReadOnly Property MaximumHP As Long
    ReadOnly Property IsDead As Boolean
    ReadOnly Property NeedsHealing As Boolean
    Sub Heal()
End Interface
