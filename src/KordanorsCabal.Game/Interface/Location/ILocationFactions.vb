Public Interface ILocationFactions
    Inherits IBaseThingie
    Function AlliesOf(character As ICharacter) As IEnumerable(Of ICharacter)
    Function FirstEnemy(character As ICharacter) As ICharacter
    Function EnemiesOf(character As ICharacter) As IEnumerable(Of ICharacter)
End Interface
