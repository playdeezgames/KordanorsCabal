Public Interface ICharacterQuest
    Inherits IBaseThingie
    Sub Accept(quest As IQuestType)
    Function CanAccept(quest As IQuestType) As Boolean
    Sub Complete(quest As IQuestType)
    Function Has(quest As IQuestType) As Boolean
End Interface
