Public Interface IQuestType
    Inherits IBaseThingie
    Function CanAccept(character As ICharacter) As Boolean
    Function CanComplete(character As ICharacter) As Boolean
    Sub Accept(character As ICharacter)
    Sub Complete(character As ICharacter)
End Interface
