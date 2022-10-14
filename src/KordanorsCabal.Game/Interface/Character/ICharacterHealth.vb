Public Interface ICharacterHealth
    Inherits IBaseThingie
    Property Current As Long
    ReadOnly Property Maximum As Long
    ReadOnly Property IsDead As Boolean
    ReadOnly Property NeedsHealing As Boolean
    Sub Heal()
End Interface
