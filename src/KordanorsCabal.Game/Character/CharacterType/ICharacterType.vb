Public Interface ICharacterType
    Inherits IBaseThingie
    ReadOnly Property IsUndead As Boolean
    ReadOnly Property Name As String
    ReadOnly Property Spawning As ICharacterTypeSpawning
    ReadOnly Property Combat As ICharacterTypeCombat
    'combat
    Function CanBeBribedWith(itemType As OldItemType) As Boolean
    Sub DropLoot(location As ILocation)
    Function GenerateAttackType() As AttackType
    Function IsEnemy(character As ICharacterType) As Boolean
    Function PartingShot() As String
    Function RollMoneyDrop() As Long
    ReadOnly Property XPValue As Long
End Interface
