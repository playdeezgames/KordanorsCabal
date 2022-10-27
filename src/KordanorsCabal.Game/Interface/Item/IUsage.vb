Public Interface IUsage
    Inherits IBaseThingie
    ReadOnly Property IsConsumed As Boolean
    ReadOnly Property CanUse(character As ICharacter) As Boolean
    Sub Use(character As ICharacter)
    Sub Purify()
    Sub Decay()
End Interface
