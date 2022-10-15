Public Interface ICharacterPhysicalCombat
    Inherits IBaseThingie
    Function Kill(killedBy As ICharacter) As (Sfx?, List(Of String))
    ReadOnly Property CanFight As Boolean
    ReadOnly Property IsEnemy(character As ICharacter) As Boolean
    ReadOnly Property PartingShot As String
    Sub Fight()
    Sub Run()
    Function DetermineDamage(value As Long) As Long
    Sub DoDamage(damage As Long)
    Sub DoCounterAttacks()
    Function RollDefend() As Long
    Function RollAttack() As Long
End Interface
