Public Interface ICharacterType
    Inherits IBaseThingie
    ReadOnly Property IsUndead As Boolean
    ReadOnly Property Name As String
    ReadOnly Property Spawning As ICharacterTypeSpawning
    ReadOnly Property Combat As ICharacterTypeCombat
    Function IsEnemy(character As ICharacterType) As Boolean
    Function PartingShot() As String
    Function RollMoneyDrop() As Long
    ReadOnly Property XPValue As Long
End Interface
