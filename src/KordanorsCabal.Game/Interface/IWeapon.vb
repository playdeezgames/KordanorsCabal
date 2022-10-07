Public Interface IWeapon
    Inherits IBaseThingie
    ReadOnly Property IsWeapon() As Boolean
    ReadOnly Property AttackDice As Long
    ReadOnly Property MaximumDamage As Long?
End Interface
