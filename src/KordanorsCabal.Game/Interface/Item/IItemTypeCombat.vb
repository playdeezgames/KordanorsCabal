Public Interface IItemTypeCombat
    Inherits IBaseThingie
    ReadOnly Property AttackDice As Long
    ReadOnly Property IsWeapon As Boolean
    ReadOnly Property MaximumDamage As Long?
    ReadOnly Property DefendDice As Long
    ReadOnly Property IsArmor As Boolean
End Interface
