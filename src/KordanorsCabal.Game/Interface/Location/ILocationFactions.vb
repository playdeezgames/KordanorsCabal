Public Interface ILocationFactions
    Inherits IBaseThingie
    Function Allies(character As ICharacter) As IEnumerable(Of ICharacter)
    Function Enemy(character As ICharacter) As ICharacter
    Function Enemies(character As ICharacter) As IEnumerable(Of ICharacter)
End Interface
